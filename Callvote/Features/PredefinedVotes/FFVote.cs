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
    public class FFVote(Player player) : BinaryVote(player, ReplacePlayer(player), nameof(VoteTypeEnum.Ff), AddCallback), IPredefinedVote
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
                    ? Translation.DisablingFriendlyFire
                    : Translation.EnablingFriendlyFire;

                message = message.Replace("%VotePercent%", yesVotePercent.ToString());

                Server.FriendlyFire = !Server.FriendlyFire;
            }
            else
            {
                message = Server.FriendlyFire
                    ? Translation.NoSuccessFullEnableFf
                    : Translation.NoSuccessFullDisableFf;

                message = message
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdFF%", Config.ThresholdFf.ToString());
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.FinalResultsDuration),
                $"<size={DisplayMessageHelper.CalculateMessageSize(message)}>{message}</size>",
                vote.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player)
        {
            string baseQuestion = Server.FriendlyFire
                ? Translation.AskedToDisableFf
                : Translation.AskedToEnableFf;

            return baseQuestion.Replace("%Player%", player.Nickname);
        }
    }
}
