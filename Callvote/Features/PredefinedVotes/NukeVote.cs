#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API.VoteTemplate;
using Callvote.Configuration;
using Callvote.Features.Enums;
using Callvote.Features.Interfaces;

namespace Callvote.Features.PredefinedVotes
{
    /// <summary>
    /// Represents the type for the Nuke Predefined Vote.
    /// Initializes a new instance of the <see cref="NukeVote"/> class.
    /// </summary>
    /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
    public class NukeVote(Player player) : BinaryVote(player, ReplacePlayer(player), nameof(VoteTypeEnum.Nuke), AddCallback), IPredefinedVote
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        private static void AddCallback(Vote vote)
        {
            if (vote is not BinaryVote binaryVote)
            {
                return;
            }

            int yesVotePercent = vote.GetVoteOptionPercentage(binaryVote.YesVoteOption);
            int noVotePercent = vote.GetVoteOptionPercentage(binaryVote.NoVoteOption);

            string message;

            if (yesVotePercent >= Config.ThresholdNuke && yesVotePercent > noVotePercent)
            {
                message = Translation.FoundationNuked.Replace("%VotePercent%", yesVotePercent.ToString());

                Warhead.Detonate();
            }
            else
            {
                message = Translation.NoSuccessFullNuke
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdNuke%", Config.ThresholdNuke.ToString());
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.FinalResultsDuration),
                $"<size={DisplayMessageHelper.CalculateMessageSize(message)}>{message}</size>",
                vote.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player)
        {
            return Translation.AskedToNuke.Replace("%Player%", player.Nickname);
        }
    }
}
