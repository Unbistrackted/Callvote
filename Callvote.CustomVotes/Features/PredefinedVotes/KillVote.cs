using Callvote.API.Features.Displays;
using Callvote.API.Features.Votes;
using Callvote.API.Interfaces;
using Callvote.CustomVotes.Configuration;
using Callvote.Features.VoteTemplate;
using LabApi.Features.Wrappers;

namespace Callvote.CustomVotes.Features.PredefinedVotes
{
    /// <summary>
    /// Represents the type for the Kill Player Predefined Vote.
    /// Initializes a new instance of the <see cref="KillVote"/> class.
    /// </summary>
    /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
    /// <param name="ofender">The <see cref="Player"/> that is going to be killed.</param>
    /// <param name="reason">The reason for the kick.</param>
    public class KillVote(Player player, Player ofender, string reason) : BinaryVote(player, ReplacePlayer(player, ofender, reason), nameof(CustomVoteType.Kill), vote => AddCallback(vote, player, ofender, reason)), IPredefinedVote
    {
        private static Translation Translation => Plugin.Instance.Translation;

        private static Config Config => Plugin.Instance.Config;

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

            DisplayHandler.Show(CallvotePlugin.Instance.Config.FinalResultsDuration, message, vote.AllowedPlayers);
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
