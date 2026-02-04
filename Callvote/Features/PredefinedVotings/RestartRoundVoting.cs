#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.Features.Enums;
using System;
using System.Linq;
using Callvote.API.VotingsTemplate;
using Callvote.Features.Interfaces;
using Callvote.API;

namespace Callvote.Features.PredefinedVotings
{
    public class RestartRoundVoting(Player player) : BinaryVoting(player, ReplacePlayer(player), nameof(VotingTypeEnum.RestartRound), AddCallback), IVotingTemplate
    {
        public static void AddCallback(Voting vote)
        {
            int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);

            if (yesVotePercent >= Callvote.Instance.Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.RoundRestarting)}>{Callvote.Instance.Translation.RoundRestarting
                    .Replace("%VotePercent%", yesVotePercent.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
                Round.Restart();
            }
            else
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.NoSuccessFullRestart)}>{Callvote.Instance.Translation.NoSuccessFullRestart
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdRestartRound%", Callvote.Instance.Config.ThresholdRestartRound.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
        }

        private static string ReplacePlayer(Player player)
        {
            return Callvote.Instance.Translation.AskedToRestart.Replace("%Player%", player.Nickname);
        }
    }
}
