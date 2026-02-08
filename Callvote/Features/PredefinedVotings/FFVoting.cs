#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API.VotingsTemplate;
using Callvote.Configuration;
using Callvote.Features.Enums;
using Callvote.Features.Interfaces;

namespace Callvote.Features.PredefinedVotings
{
    /// <summary>
    /// Represents the type for the Friendly Fire enable/disable Predefined Voting.
    /// Initializes a new instance of the <see cref="FFVoting"/> class.
    /// </summary>
    /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
    public class FFVoting(Player player) : BinaryVoting(player, ReplacePlayer(player), nameof(VotingTypeEnum.Ff), AddCallback), IVotingTemplate
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        private static void AddCallback(Voting voting)
        {
            if (voting is not BinaryVoting binaryVoting)
            {
                return;
            }

            int yesVotePercent = voting.GetVotePercentage(binaryVoting.YesVote);
            int noVotePercent = voting.GetVotePercentage(binaryVoting.NoVote);

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
                voting.AllowedPlayers);
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
