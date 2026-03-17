using Callvote.API.Features.Votes;
using Discord;
using DiscordLab.Bot;
using Config = Callvote.DiscordLab.Configuration.Config;
using LabApi.Features.Console;
using System.Text.RegularExpressions;
using Discord.Rest;
using System;

namespace Callvote.DiscordLab.Features
{
    internal static class DiscordLabEmbed
    {
        private static Config Config => Plugin.Instance.Config;

        internal static void SendVoteResults(Vote vote)
        {
            try
            {
                if (!Client.IsClientReady)
                {
                    Logger.Warn($"DiscordLab Bot was not initialized when the voting ended!");
                    return;
                }
            }
            catch
            {
                Logger.Warn($"DiscordLab Bot was not initialized when the voting ended!");
                return;
            }

            _ = Client.GetOrAddChannel(Config.DiscordChannelId)?.SendMessageAsync(embed: BuildEmbed(vote));
        }

        private static Embed BuildEmbed(Vote vote)
        {
            string configJson = Config.EmbedJson
                .Replace("%player%", vote.CallVotePlayer?.Username ?? vote.CallVotePlayer?.UniqueUserId)
                .Replace("%question%", RemoveColorTags(vote?.Question))
                .Replace("%winningVoteOption%", vote.GetWinningVoteOption().Option)
                .Replace("%winningVoteOptionColor%", GetColor(vote.GetWinningVoteOption()?.Detail).ToString());

            if (string.IsNullOrEmpty(configJson) || !EmbedBuilderUtils.TryParse(configJson, out EmbedBuilder embedBuilder))
            {
                embedBuilder = new()
                {
                    Title = Config.EmbedTitle,
                    Color = GetColor(vote.GetWinningVoteOption()?.Detail),
                };

                embedBuilder.AddField(Config.EmbedPlayer, vote.CallVotePlayer?.Username ?? vote.CallVotePlayer?.UniqueUserId);
                embedBuilder.AddField(Config.EmbedQuestion, RemoveColorTags(vote?.Question));
            }

            foreach (VoteOption option in vote.VoteOptions)
            {
                embedBuilder.AddField(RemoveColorTags(option.Detail), vote.Counter[option].ToString(), true);
            }

            return embedBuilder.Build();
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
    }
}
