using Callvote.Configuration;
using Callvote.Features;
using Callvote.SoftDependencies.Interfaces;
using Google.Protobuf.Collections;
using SCPDiscord.Interface;
using System;
using System.Text.RegularExpressions;

namespace Callvote.SoftDependencies.DiscordEmbedProviders
{
    /// <summary>
    /// Represents the type that sends a vote result via DiscordLab Webhook using DiscordLab's Bot.
    /// </summary>
    public class ScpDiscordMessageProvider : IWebhookProvider
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        /// <inheritdoc/>
        public void SendVoteResults(Vote vote)
        {
            if (!SCPDiscord.NetworkSystem.IsConnected())
            {
                ServerConsole.AddLog($"[ERROR] [Callvote] DiscordLab was not initialized!", ConsoleColor.Red);
                return;
            }

            RepeatedField<EmbedMessage.Types.DiscordEmbedField> fields = [];

            // Player who called the vote
            fields.Add(new EmbedMessage.Types.DiscordEmbedField()
            {
                Name = Translation.WebhookPlayer,
                Value = vote.CallVotePlayer?.Nickname ?? vote.CallVotePlayerId,
                Inline = false,
            });

            // Vote Question
            fields.Add(new EmbedMessage.Types.DiscordEmbedField()
            {
                Name = Translation.WebhookQuestion,
                Value = RemoveColorTags(vote?.Question),
                Inline = false,
            });

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

            EmbedMessage embedMessage = new()
            {
                Title = Translation.WebhookTitle,
                ChannelID = Config.DiscordChannelId,
                Colour = GetColor(vote.GetWinningVoteOption()?.Detail),
            };

            embedMessage.Fields.AddRange(fields);

            try
            {
                SCPDiscord.SCPDiscord.SendEmbedByID(embedMessage);
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
