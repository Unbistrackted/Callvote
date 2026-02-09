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
    /// Represents the type for the Restart Round Predefined Vote.
    /// Initializes a new instance of the <see cref="RestartRoundVote"/> class.
    /// </summary>
    /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
    public class RestartRoundVote(Player player) : BinaryVote(player, ReplacePlayer(player), nameof(Enums.VoteType.RestartRound), AddCallback), IPredefinedVote
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

            if (yesVotePercent >= Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
            {
                message = Translation.RoundRestarting
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
                message = Translation.NoSuccessFullRestart
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdRestartRound%", Config.ThresholdRestartRound.ToString())
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail)
                    .Replace("%VoteDetail%", binaryVote.YesVoteOption.Detail);
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.FinalResultsDuration),
                $"<size={DisplayMessageHelper.CalculateMessageSize(message)}>{message}</size>",
                vote.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player)
        {
            return Translation.AskedToRestart.Replace("%Player%", player.Nickname);
        }
    }
}
