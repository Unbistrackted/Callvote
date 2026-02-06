#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Linq;
using Callvote.API;
using Callvote.API.VotingsTemplate;
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
        private static void AddCallback(Voting vote)
        {
            int yesVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);

            if (yesVotePercent >= CallvotePlugin.Instance.Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
            {
                SoftDependency.MessageProvider.DisplayMessage(
                    TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                    $"<size={DisplayMessageHelper.CalculateMessageSize(CallvotePlugin.Instance.Translation.RoundRestarting)}>{CallvotePlugin.Instance.Translation.RoundRestarting
                    .Replace("%VotePercent%", yesVotePercent.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
                Round.Restart();
            }
            else
            {
                SoftDependency.MessageProvider.DisplayMessage(
                    TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                    $"<size={DisplayMessageHelper.CalculateMessageSize(CallvotePlugin.Instance.Translation.NoSuccessFullRestart)}>{CallvotePlugin.Instance.Translation.NoSuccessFullRestart
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdRestartRound%", CallvotePlugin.Instance.Config.ThresholdRestartRound.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
        }

        private static string ReplacePlayer(Player player)
        {
            return CallvotePlugin.Instance.Translation.AskedToRestart.Replace("%Player%", player.Nickname);
        }
    }
}
