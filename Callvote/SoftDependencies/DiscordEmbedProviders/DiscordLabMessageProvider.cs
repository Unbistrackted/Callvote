using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Callvote.Configuration;
using Callvote.Features;
using Callvote.SoftDependencies.Interfaces;
using Discord;
using DiscordLab.Bot;
using Config = Callvote.Configuration.Config;

namespace Callvote.SoftDependencies.DiscordEmbedProviders
{
    /// <summary>
    /// Represents the type that sends a vote result via DiscordLab Webhook using DiscordLab's Bot.
    /// </summary>
    internal class DiscordLabMessageProvider : IWebhookProvider
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        /// <inheritdoc/>
        public void SendVoteResults(Vote vote)
        {
            if (!Client.IsClientReady)
            {
                ServerConsole.AddLog($"[ERROR] [Callvote] DiscordLab was not initialized!", ConsoleColor.Red);
                return;
            }

            List<EmbedFieldBuilder> fields = [];

            // Player who called the vote
            fields.Add(new EmbedFieldBuilder()
            {
                Name = Translation.WebhookPlayer,
                Value = vote.CallVotePlayer?.Nickname ?? vote.CallVotePlayerId,
                IsInline = false,
            });

            // Vote Question
            fields.Add(new EmbedFieldBuilder()
            {
                Name = Translation.WebhookQuestion,
                Value = RemoveColorTags(vote?.Question),
                IsInline = false,
            });

            // Vote Options and Counters
            foreach (VoteOption option in vote.VoteOptions)
            {
                fields.Add(new EmbedFieldBuilder()
                {
                    Name = RemoveColorTags(option.Detail),
                    Value = vote.Counter[option].ToString(),
                    IsInline = true,
                });
            }

            EmbedBuilder embedBuilder = new()
            {
                Title = Translation.WebhookTitle,
                Color = GetColor(vote.GetWinningVoteOption()?.Detail),
                Fields = fields,
            };

            try
            {
                Client.GetOrAddChannel(Config.DiscordChannelId).SendMessageAsync(embed: embedBuilder.Build());
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

        private static Color GetColor(string option)
        {
            if (string.IsNullOrEmpty(option))
            {
                return Color.Default;
            }

            string color = Regex.Match(option, "<color=(.*?)>").Groups[1].Value;

            if (Color.TryParse(color, out Color parsedColor))
            {
                return parsedColor;
            }

            return Color.Parse(GameConsoleTransmission.ProcessColor(color).ToHex().Remove(7));
        }
    }
}
