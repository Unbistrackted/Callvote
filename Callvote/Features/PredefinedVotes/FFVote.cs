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
    /// Represents the type for the Friendly Fire enable/disable Predefined Vote.
    /// Initializes a new instance of the <see cref="FFVote"/> class.
    /// </summary>
    /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
    public class FFVote(Player player) : BinaryVote(player, ReplacePlayer(player), nameof(Enums.VoteType.Ff), AddCallback), IPredefinedVote
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

            if (yesVotePercent >= Config.ThresholdFf && yesVotePercent > noVotePercent)
            {
                message = Server.FriendlyFire
                    ? Translation.EnablingFriendlyFire
                    : Translation.DisablingFriendlyFire;

                message = message
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);

                Server.FriendlyFire = !Server.FriendlyFire;
            }
            else
            {
                message = Server.FriendlyFire
                    ? Translation.NoSuccessFullDisableFf
                    : Translation.NoSuccessFullEnableFf;

                message = message
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdFF%", Config.ThresholdFf.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.FinalResultsDuration),
                $"<size={DisplayMessageHelper.CalculateMessageSize(message)}>{message}</size>",
                vote.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player)
        {
            string baseQuestion = Server.FriendlyFire // Not sure how I would do that but if the vote was queued and the FF state changed, the question would not reflect the new state.
                ? Translation.AskedToDisableFf
                : Translation.AskedToEnableFf;

            return baseQuestion.Replace("%Player%", player.Nickname);
        }
    }
}
