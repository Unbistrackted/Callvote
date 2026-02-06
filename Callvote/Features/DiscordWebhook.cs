using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Callvote.Features
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only public API documentation is required")]
    internal static class DiscordWebhook
    {
        internal static async Task ResultsMessage(Voting vote)
        {
            string webhook = CallvotePlugin.Instance.Config.DiscordWebhook;

            if (string.IsNullOrWhiteSpace(webhook))
            {
                return;
            }

            string resultsMessage = string.Empty;

            foreach (KeyValuePair<string, string> kvp in vote.Options)
            {
                resultsMessage += CallvotePlugin.Instance.Translation.OptionAndCounter
                    .Replace("%Option%", kvp.Value)
                    .Replace("%OptionKey%", kvp.Key)
                    .Replace("%Counter%", vote.Counter[kvp.Key].ToString());
            }

            string question = Escape(vote.Question);
            string results = Escape(resultsMessage);
            string callvotePlayerInfo = Escape($"{vote.CallVotePlayer.Nickname}");
            string payload = $@"{{""content"":null,""embeds"":[{{""title"":""{CallvotePlugin.Instance.Translation.WebhookTitle}"",""color"":255,""fields"":[{{""name"":""{CallvotePlugin.Instance.Translation.WebhookPlayer}"",""value"":""{callvotePlayerInfo}""}},{{""name"":""{CallvotePlugin.Instance.Translation.WebhookQuestion}"",""value"":""{question.Replace($"{callvotePlayerInfo} asks: ", string.Empty)}""}},{{""name"":""{CallvotePlugin.Instance.Translation.WebhookVotes}"",""value"":""{results}""}}]}}]}}";
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
