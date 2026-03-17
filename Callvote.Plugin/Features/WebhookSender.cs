using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Callvote.API.Features.Votes;
using Callvote.Configuration;
using LabApi.Features.Console;

namespace Callvote.Features
{
    /// <summary>
    /// Represents the type that sends a vote result via Callvote's implementation of a webhook.
    /// </summary>
    internal class WebhookSender
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        public static void SendVoteResults(Vote vote)
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
                    .Replace("%VoteCommand%", voteOption.VoteCommand.Command)
                    .Replace("%VoteDetail%", voteOption.Detail)
                    .Replace("%VoteCounter%", vote.Counter[voteOption].ToString());
            }

            string question = Escape(vote.Question);
            string results = Escape(resultsMessage);
            string callvotePlayerInfo = Escape($"{vote.CallVotePlayer.Username}");
            string payload = $@"{{""content"":null,""embeds"":[{{""title"":""{Translation.WebhookTitle}"",""color"":255,""fields"":[{{""name"":""{Translation.WebhookPlayer}"",""value"":""{callvotePlayerInfo}""}},{{""name"":""{Translation.WebhookQuestion}"",""value"":""{question.Replace($"{callvotePlayerInfo} asks: ", string.Empty)}""}},{{""name"":""{Translation.WebhookVotes}"",""value"":""{results}""}}]}}]}}";

            _ = SendWebhook(payload, Config.DiscordWebhook);
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

        private static async Task SendWebhook(string payload, string webhook)
        {
            try
            {
                HttpClient httpClient = new();

                var request = new StringContent(payload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(webhook, request);

                if (!response.IsSuccessStatusCode)
                {
                    Logger.Error($"Webhook Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Webhook Error: " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
