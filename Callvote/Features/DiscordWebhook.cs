using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Callvote.Configuration;

namespace Callvote.Features
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only public API documentation is required")]
    internal static class DiscordWebhook
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        internal static async Task ResultsMessage(Voting voting)
        {
            string webhook = Config.DiscordWebhook;

            if (string.IsNullOrWhiteSpace(webhook))
            {
                return;
            }

            string resultsMessage = string.Empty;

            foreach (Vote vote in voting.VoteOptions)
            {
                resultsMessage += Translation.OptionAndCounter
                    .Replace("%VoteCommand%", vote.Command.Command)
                    .Replace("%VoteDetail%", vote.Detail)
                    .Replace("%VoteCounter%", voting.Counter[vote].ToString());
            }

            string question = Escape(voting.Question);
            string results = Escape(resultsMessage);
            string callvotePlayerInfo = Escape($"{voting.CallVotePlayer.Nickname}");
            string payload = $@"{{""content"":null,""embeds"":[{{""title"":""{Translation.WebhookTitle}"",""color"":255,""fields"":[{{""name"":""{Translation.WebhookPlayer}"",""value"":""{callvotePlayerInfo}""}},{{""name"":""{Translation.WebhookQuestion}"",""value"":""{question.Replace($"{callvotePlayerInfo} asks: ", string.Empty)}""}},{{""name"":""{Translation.WebhookVotes}"",""value"":""{results}""}}]}}]}}";
            try
            {
                using (HttpClient client = new())
                {
                    var request = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(webhook, request);

                    if (!response.IsSuccessStatusCode)
                    {
                        ServerConsole.AddLog($"[ERROR] [Callvote] Webhook Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}", ConsoleColor.Red);
                        return;
                    }
                }
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
    }
}
