using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Callvote.Features
{
    public static class DcWebhook
    {
        public static void ResultsMessage(Voting vote)
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
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                client.UploadString(webhook, "POST", payload);
            }
            catch (Exception ex)
            {
                Log.Error("Webhook Error: " + ex.Message);
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
