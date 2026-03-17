using Callvote.API.Features.Displays;
using Callvote.API.Features.Votes;
using Callvote.API.Interfaces;
using Callvote.Features.VoteTemplate;
using LabApi.Features.Wrappers;

namespace Callvote.CustomVotes.Features.PredefinedVotes
{
    /// <summary>
    /// Represents the type for the Kick Player Predefined Vote.
    /// Initializes a new instance of the <see cref="KickVote"/> class.
    /// </summary>
    /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
    /// <param name="ofender">The <see cref="Player"/> that is going to be kicked.</param>
    /// <param name="reason">The reason for the kick.</param>
    public class KickVote(Player player, Player ofender, string reason) : BinaryVote(player, ReplacePlayer(player, ofender, reason), nameof(CustomVoteType.Kick), vote => AddCallback(vote, player, ofender, reason)), IPredefinedVote
    {
        private static void AddCallback(Vote vote, Player player, Player ofender, string reason)
        {
            if (vote is not BinaryVote binaryVote)
            {
                return;
            }

            int yesVotePercent = vote.GetVoteOptionPercentage(binaryVote.YesVoteOption);
            int noVotePercent = vote.GetVoteOptionPercentage(binaryVote.NoVoteOption);

            string message;

            if (yesVotePercent >= Plugin.Instance.Config.ThresholdKick && yesVotePercent > noVotePercent)
            {
                message = Plugin.Instance.Translation.PlayerKicked
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", ofender.Nickname)
                    .Replace("%Reason%", reason);

                ofender.Kick(reason);
            }
            else
            {
                message = Plugin.Instance.Translation.NotSuccessFullKick
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdKick%", Plugin.Instance.Config.ThresholdKick.ToString())
                    .Replace("%Offender%", ofender.Nickname)
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);
            }

            DisplayHandler.Show(CallvotePlugin.Instance.Config.FinalResultsDuration, message, vote.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player, Player offender, string reason)
        {
            return Plugin.Instance.Translation.AskedToKick
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", offender.Nickname)
                    .Replace("%Reason%", reason);
        }
    }
}
