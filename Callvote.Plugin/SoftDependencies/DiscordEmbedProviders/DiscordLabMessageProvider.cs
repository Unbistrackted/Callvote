using System;
using System.Text.RegularExpressions;
using Callvote.API.Features.Votes;
using Callvote.Configuration;
using Discord;
using DiscordLab.Bot;
using Respawning.Objectives;
using Config = Callvote.Configuration.Config;
using EmbedProvider = Callvote.SoftDependencies.DiscordEmbed.EmbedProvider;

namespace Callvote.SoftDependencies.DiscordEmbedProviders
{
    /// <summary>
    /// Represents the type that sends a vote result via DiscordLab Webhook using DiscordLab's Bot.
    /// </summary>
    internal class DiscordLabMessageProvider : EmbedProvider
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        /// <inheritdoc/>
        public override void SendVoteResults(Vote vote)
        {
            if (!Client.IsClientReady)
            {
                ServerConsole.AddLog($"[ERROR] [Callvote] DiscordLab was not initialized!", ConsoleColor.Red);
                return;
            }

            _ = Client.GetOrAddChannel(Config.DiscordChannelId)?.SendMessageAsync(embed: BuildEmbed(vote));
        }

        private static string RemoveColorTags(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

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

        private static Embed BuildEmbed(Vote vote)
        {
            EmbedBuilder embedBuilder = new()
            {
                Title = Translation.WebhookTitle,
                Color = GetColor(vote.GetWinningVoteOption()?.Detail),
            };

            embedBuilder.AddField(Translation.WebhookPlayer, vote.CallVotePlayer?.Username ?? vote.CallVotePlayer?.UniqueUserId);
            embedBuilder.AddField(Translation.WebhookQuestion, RemoveColorTags(vote?.Question));

            foreach (VoteOption option in vote.VoteOptions)
            {
                embedBuilder.AddField(RemoveColorTags(option.Detail), vote.Counter[option].ToString(), true);
            }

            return embedBuilder.Build();
        }
    }
}
