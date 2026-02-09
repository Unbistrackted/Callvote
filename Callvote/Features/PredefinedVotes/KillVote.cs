#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Permissions;
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
    /// Represents the type for the Kill Player Predefined Vote.
    /// Initializes a new instance of the <see cref="KillVote"/> class.
    /// </summary>
    /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
    /// <param name="ofender">The <see cref="Player"/> that is going to be killed.</param>
    /// <param name="reason">The reason for the kick.</param>
    public class KillVote(Player player, Player ofender, string reason) : BinaryVote(player, ReplacePlayer(player, ofender, reason), nameof(Enums.VoteType.Kill), vote => AddCallback(vote, player, ofender, reason)), IPredefinedVote
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        private static void AddCallback(Vote vote, Player player, Player ofender, string reason)
        {
            if (vote is not BinaryVote binaryVote)
            {
                return;
            }

            int yesVotePercent = vote.GetVoteOptionPercentage(binaryVote.YesVoteOption);
            int noVotePercent = vote.GetVoteOptionPercentage(binaryVote.NoVoteOption);

            string message;

            if (yesVotePercent >= Config.ThresholdKill && yesVotePercent > noVotePercent)
            {
                message = Translation.PlayerKilled
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", ofender.Nickname)
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail)
                    .Replace("%Reason%", reason);

                ofender.Kill(reason);
            }
            else
            {
                message = Translation.NoSuccessFullKill
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdKill%", Config.ThresholdKick.ToString())
                    .Replace("%Offender%", ofender.Nickname)
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.FinalResultsDuration),
                $"<size={DisplayMessageHelper.CalculateMessageSize(message)}>{message}</size>",
                vote.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player, Player offender, string reason)
        {
            return Translation.AskedToKill
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", offender.Nickname)
                    .Replace("%Reason%", reason);
        }
    }
}
