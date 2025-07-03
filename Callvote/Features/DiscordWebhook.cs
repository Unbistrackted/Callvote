using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Callvote.Features
{
    public static class DiscordWebhook
    {
        public static async Task ResultsMessage(Voting vote)
        {
            string webhook = Callvote.Instance.Config.DiscordWebhook;
            if (string.IsNullOrWhiteSpace(webhook)) { return; }
            string resultsMessage = "";
            foreach (KeyValuePair<string, string> kvp in vote.Options)
            {
                resultsMessage += Callvote.Instance.Translation.OptionAndCounter
                    .Replace("%Option%", kvp.Value)
                    .Replace("%OptionKey%", kvp.Key)
                    .Replace("%Counter%", vote.Counter[kvp.Key].ToString());
            }
            string Question = Escape(vote.Question);
            string Results = Escape(resultsMessage);
            string CallvotePlayerInfo = Escape($"{vote.CallVotePlayer.Nickname}");
            string payload = $@"{{""content"":null,""embeds"":[{{""title"":""{Callvote.Instance.Translation.WebhookTitle}"",""color"":255,""fields"":[{{""name"":""{Callvote.Instance.Translation.WebhookPlayer}"",""value"":""{CallvotePlayerInfo}""}},{{""name"":""{Callvote.Instance.Translation.WebhookQuestion}"",""value"":""{Question.Replace($"{CallvotePlayerInfo} asks: ", "")}""}},{{""name"":""{Callvote.Instance.Translation.WebhookVotes}"",""value"":""{Results}""}}]}}]}}";
            try
            {
                using (HttpClient client = new HttpClient())
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
            return Regex.Replace(input, "<color=.*?>|</color>", "");
        }
        private static string Escape(string message)
        {
            message = RemoveColorTags(message);
            if (string.IsNullOrEmpty(message)) { return ""; }
            return message
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r");
        }
    }
}
