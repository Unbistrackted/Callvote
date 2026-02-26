#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.Features.VoteTemplate;
using Callvote.API.Enums;
using Callvote.API.Interfaces;
using Callvote.API.Features.Votes;
using Callvote.API.Features.DisplayMessage;

namespace Callvote.Features.PredefinedVotes
{
    /// <summary>
    /// Represents the type for the Friendly Fire enable/disable Predefined Vote.
    /// Initializes a new instance of the <see cref="FFVote"/> class.
    /// </summary>
    /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
    public class FFVote(Player player) : BinaryVote(player, ReplacePlayer(player), nameof(VoteType.Ff), AddCallback), IPredefinedVote
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

            if (yesVotePercent >= CallvotePlugin.Instance.Config.ThresholdFf && yesVotePercent > noVotePercent)
            {
                message = Server.FriendlyFire
                    ? CallvotePlugin.Instance.Translation.EnablingFriendlyFire
                    : CallvotePlugin.Instance.Translation.DisablingFriendlyFire;

                message = message
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);

                Server.FriendlyFire = !Server.FriendlyFire;
            }
            else
            {
                message = Server.FriendlyFire
                    ? CallvotePlugin.Instance.Translation.NoSuccessFullDisableFf
                    : CallvotePlugin.Instance.Translation.NoSuccessFullEnableFf;

                message = message
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdFF%", CallvotePlugin.Instance.Config.ThresholdFf.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);
            }

            DisplayHandler.Show(CallvotePlugin.Instance.Config.FinalResultsDuration, message, vote.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player)
        {
            string baseQuestion = Server.FriendlyFire // Not sure how I would do that but if the vote was queued and the FF state changed, the question would not reflect the new state.
                ? CallvotePlugin.Instance.Translation.AskedToDisableFf
                : CallvotePlugin.Instance.Translation.AskedToEnableFf;

            return baseQuestion.Replace("%Player%", player.Nickname);
        }
    }
}
