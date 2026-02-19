using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Callvote.Configuration;
using Callvote.Features;
using Callvote.SoftDependencies.Interfaces;

namespace Callvote.SoftDependencies.DiscordEmbedProviders
{
    /// <summary>
    /// Represents the type that sends a vote result via Callvote's implementation of a webhook.
    /// </summary>
    internal class WebhookProvider : IWebhookProvider
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        /// <inheritdoc/>
        public void SendVoteResults(Vote vote)
        {
            // Made by Playeroth and Edi, and I'm too lazy to organize this mess
            if (string.IsNullOrWhiteSpace(Config.DiscordWebhook))
            {
                return;
            }

            string resultsMessage = string.Empty;

            foreach (VoteOption voteOption in vote.VoteOptions)
            {
                resultsMessage += Translation.OptionAndCounter
                    .Replace("%VoteCommand%", voteOption.Command.Command)
                    .Replace("%VoteDetail%", voteOption.Detail)
                    .Replace("%VoteCounter%", vote.Counter[voteOption].ToString());
            }

            string question = Escape(vote.Question);
            string results = Escape(resultsMessage);
            string callvotePlayerInfo = Escape($"{vote.CallVotePlayer.Nickname}");
            string payload = $@"{{""content"":null,""embeds"":[{{""title"":""{Translation.WebhookTitle}"",""color"":255,""fields"":[{{""name"":""{Translation.WebhookPlayer}"",""value"":""{callvotePlayerInfo}""}},{{""name"":""{Translation.WebhookQuestion}"",""value"":""{question.Replace($"{callvotePlayerInfo} asks: ", string.Empty)}""}},{{""name"":""{Translation.WebhookVotes}"",""value"":""{results}""}}]}}]}}";
            try
            {
                _ = Task.Run(async () => await this.SendWebhook(payload, Config.DiscordWebhook));
            }
            catch (Exception ex)
            {
                ServerConsole.AddLog($"[ERROR] [Callvote] " + "Webhook Error: " + ex.Message + " " + ex.Source + " " + ex.StackTrace, ConsoleColor.Red);
            }
        }

        private static string RemoveColorTags(string input)
        {
            return Regex.Replace(input, "<color=.*?>|</color>", string.Empty);
        }

        private static string Escape(string message)
        {
            message = RemoveColorTags(message);

            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }

            return message
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r");
        }

        private async Task SendWebhook(string payload, string webhook)
        {
            using HttpClient client = new();
            var request = new StringContent(payload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(webhook, request);

            if (!response.IsSuccessStatusCode)
            {
                ServerConsole.AddLog($"[ERROR] [Callvote] Webhook Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}", ConsoleColor.Red);
                return;
            }
        }
    }
}
