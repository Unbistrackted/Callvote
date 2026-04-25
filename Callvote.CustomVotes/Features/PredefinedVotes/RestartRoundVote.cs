#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.API.Features.Displays;
using Callvote.API.Features.Votes;
using Callvote.API.Interfaces;
using Callvote.Features.VoteTemplate;

namespace Callvote.CustomVotes.Features.PredefinedVotes
{
    /// <summary>
    /// Represents the type for the Restart Round Predefined Vote.
    /// Initializes a new instance of the <see cref="RestartRoundVote"/> class.
    /// </summary>
    /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
    public class RestartRoundVote(Player player) : BinaryVote(player, ReplacePlayer(player), nameof(CustomVoteType.RestartRound), AddCallback), IPredefinedVote
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

            if (yesVotePercent >= CustomVotePlugin.Instance.Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
            {
                message = CustomVotePlugin.Instance.Translation.RoundRestarting
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);
#if EXILED
                Round.EndRound(true);
#else
                Round.End();
#endif
            }
            else
            {
                message = CustomVotePlugin.Instance.Translation.NoSuccessFullRestart
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdRestartRound%", CustomVotePlugin.Instance.Config.ThresholdRestartRound.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail)
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);
            }

            DisplayHandler.Show(CallvotePlugin.Instance.Config.FinalResultsDuration, message, vote.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player) => CustomVotePlugin.Instance.Translation.AskedToRestart.Replace("%Player%", player.Nickname);
    }
}
