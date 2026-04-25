using Callvote.API.Features.Displays;
using Callvote.API.Features.Votes;
using Callvote.API.Interfaces;
using Callvote.Features.VoteTemplate;
using LabApi.Features.Wrappers;

namespace Callvote.CustomVotes.Features.PredefinedVotes
{
    /// <summary>
    /// Represents the type for the Friendly Fire enable/disable Predefined Vote.
    /// Initializes a new instance of the <see cref="FFVote"/> class.
    /// </summary>
    /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
    public class FFVote(Player player) : BinaryVote(player, ReplacePlayer(player), nameof(CustomVoteType.Ff), AddCallback), IPredefinedVote
    {
        private static void AddCallback(Vote vote)
        {
            if (vote is not BinaryVote binaryVote)
            {
                return;
            }

            int yesVotePercent = vote.GetVoteOptionPercentage(binaryVote.YesVoteOption);
            int noVotePercent = vote.GetVoteOptionPercentage(binaryVote.NoVoteOption);

            string message;

            if (yesVotePercent >= CustomVotePlugin.Instance.Config.ThresholdFf && yesVotePercent > noVotePercent)
            {
                message = Server.FriendlyFire
                    ? CustomVotePlugin.Instance.Translation.EnablingFriendlyFire
                    : CustomVotePlugin.Instance.Translation.DisablingFriendlyFire;

                message = message
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);

                Server.FriendlyFire = !Server.FriendlyFire;
            }
            else
            {
                message = Server.FriendlyFire
                    ? CustomVotePlugin.Instance.Translation.NoSuccessFullDisableFf
                    : CustomVotePlugin.Instance.Translation.NoSuccessFullEnableFf;

                message = message
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdFF%", CustomVotePlugin.Instance.Config.ThresholdFf.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);
            }

            DisplayHandler.Show(CallvotePlugin.Instance.Config.FinalResultsDuration, message, vote.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player)
        {
            string baseQuestion = Server.FriendlyFire // Not sure how I would do that but if the vote was queued and the FF state changed, the question would not reflect the new state.
                ? CustomVotePlugin.Instance.Translation.AskedToDisableFf
                : CustomVotePlugin.Instance.Translation.AskedToEnableFf;

            return baseQuestion.Replace("%Player%", player.Nickname);
        }
    }
}
