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
    /// Represents the type for the Restart Round Predefined Voting.
    /// Initializes a new instance of the <see cref="RestartRoundVoting"/> class.
    /// </summary>
    /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
    public class RestartRoundVoting(Player player) : BinaryVoting(player, ReplacePlayer(player), nameof(VotingTypeEnum.RestartRound), AddCallback), IVotingTemplate
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

            if (yesVotePercent >= Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
            {
                message = Translation.RoundRestarting.Replace("%VotePercent%", yesVotePercent.ToString());
#if EXILED
                Round.EndRound(true);
#else
                Round.End();
#endif
            }
            else
            {
                message = Translation.NoSuccessFullRestart
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdRestartRound%", Config.ThresholdRestartRound.ToString());
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.FinalResultsDuration),
                $"<size={DisplayMessageHelper.CalculateMessageSize(message)}>{message}</size>",
                voting.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player)
        {
            return Translation.AskedToRestart.Replace("%Player%", player.Nickname);
        }
    }
}
