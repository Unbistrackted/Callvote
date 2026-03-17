using Callvote.API.Features.Displays;
using Callvote.API.Features.Votes;
using Callvote.API.Interfaces;
using Callvote.Features.VoteTemplate;
using LabApi.Features.Wrappers;

namespace Callvote.CustomVotes.Features.PredefinedVotes
{
    /// <summary>
    /// Represents the type for the Nuke Predefined Vote.
    /// Initializes a new instance of the <see cref="NukeVote"/> class.
    /// </summary>
    /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
    public class NukeVote(Player player) : BinaryVote(player, ReplacePlayer(player), nameof(CustomVoteType.Nuke), AddCallback), IPredefinedVote
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

            if (yesVotePercent >= Plugin.Instance.Config.ThresholdNuke && yesVotePercent > noVotePercent)
            {
                message = Plugin.Instance.Translation.FoundationNuked
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);

                Warhead.Detonate();
            }
            else
            {
                message = Plugin.Instance.Translation.NoSuccessFullNuke
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdNuke%", Plugin.Instance.Config.ThresholdNuke.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);
            }

            DisplayHandler.Show(CallvotePlugin.Instance.Config.FinalResultsDuration, message, vote.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player) => Plugin.Instance.Translation.AskedToNuke.Replace("%Player%", player.Nickname);
    }
}
