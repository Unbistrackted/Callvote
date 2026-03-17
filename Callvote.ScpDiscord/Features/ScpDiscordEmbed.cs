using Callvote.API.Features.Votes;
using Callvote.ScpDiscord.Configuration;
using Google.Protobuf.Collections;
using LabApi.Features.Console;
using SCPDiscord.Interface;
using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Callvote.ScpDiscord.Features
{
    internal static class ScpDiscordEmbed
    {
        private static Config Config => Plugin.Instance.Config;

        /// <inheritdoc/>
        internal static void SendVoteResults(Vote vote)
        {
            try
            {
                if (!SCPDiscord.NetworkSystem.IsConnected())
                {
                    Logger.Warn($"SCPDiscord was not initialized when the voting ended!");
                    return;
                }
            }
            catch
            {
                Logger.Warn($"SCPDiscord was not initialized when the voting ended!");
                return;
            }

            EmbedMessage msg;

            RepeatedField<EmbedMessage.Types.DiscordEmbedField> fields = [];

            string configJson = Config.EmbedJson
                .Replace("%player%", vote.CallVotePlayer?.Username ?? vote.CallVotePlayer?.UniqueUserId)
                .Replace("%question%", RemoveColorTags(vote?.Question))
                .Replace("%winningVoteOption%", vote.GetWinningVoteOption().Option)
                .Replace("%winningVoteOptionColor%", GetColor(vote.GetWinningVoteOption()?.Detail).ToString());

            if (string.IsNullOrEmpty(configJson))
            {
                msg = EmbedMessage.Parser.ParseJson(configJson);
            }
            else
            {
                // Player who called the vote
                fields.Add(new EmbedMessage.Types.DiscordEmbedField()
                {
                    Name = Config.EmbedPlayer,
                    Value = vote.CallVotePlayer?.Username ?? vote.CallVotePlayer?.UniqueUserId,
                    Inline = false,
                });

                // Vote Question
                fields.Add(new EmbedMessage.Types.DiscordEmbedField()
                {
                    Name = Config.EmbedQuestion,
                    Value = RemoveColorTags(vote?.Question),
                    Inline = false,
                });
            }

            // Vote Options and Counters
            foreach (VoteOption option in vote.VoteOptions)
            {
                fields.Add(new EmbedMessage.Types.DiscordEmbedField()
                {
                    Name = RemoveColorTags(option.Detail),
                    Value = vote.Counter[option].ToString(),
                    Inline = true,
                });
            }

            msg = new()
            {
                Title = Config.EmbedTitle,
                ChannelID = Config.DiscordChannelId,
                Colour = GetColor(vote.GetWinningVoteOption()?.Detail),
            };

            msg.Fields.AddRange(fields);

            try
            {
                SCPDiscord.SCPDiscord.SendEmbedByID(msg);
                using FileStream fileStream = File.Create("something.json");

                JsonSerializer.Serialize(fileStream, msg, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                Logger.Error("Webhook Error: " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private static string RemoveColorTags(string input)
        {
            return Regex.Replace(input, "<color=.*?>|</color>", string.Empty);
        }

        private static EmbedMessage.Types.DiscordColour GetColor(string option)
        {
            if (string.IsNullOrEmpty(option))
            {
                return EmbedMessage.Types.DiscordColour.None;
            }

            string color = Regex.Match(option, "<color=(.*?)>").Groups[1].Value;

            return color switch
            {
                "red" => EmbedMessage.Types.DiscordColour.Red,
                "cyan" => EmbedMessage.Types.DiscordColour.Cyan,
                "blue" => EmbedMessage.Types.DiscordColour.Blue,
                "magenta" => EmbedMessage.Types.DiscordColour.Magenta,
                "white" => EmbedMessage.Types.DiscordColour.White,
                "green" => EmbedMessage.Types.DiscordColour.Green,
                "yellow" => EmbedMessage.Types.DiscordColour.Yellow,
                "black" => EmbedMessage.Types.DiscordColour.Black,
                _ => EmbedMessage.Types.DiscordColour.None,
            };
        }
    }
}
