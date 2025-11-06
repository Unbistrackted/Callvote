using Callvote.Enums;
using Callvote.Features;
using Callvote.Interfaces;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.API.VotingsTemplate
{
    public class RestartRoundVoting : Voting, IVotingTemplate
    {
        public RestartRoundVoting(Player player) : base(
            ReplacePlayer(player),
            nameof(VotingTypeEnum.RestartRound),
            player,
            AddCallback,
            AddOptions())
        {
        }

        public static void AddCallback(Voting vote)
        {
            int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
            if (yesVotePercent >= Callvote.Instance.Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.RoundRestarting)}>{Callvote.Instance.Translation.RoundRestarting
                    .Replace("%VotePercent%", yesVotePercent.ToString())}</size>");
                Round.Restart();
            }
            else
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.NoSuccessFullRestart)}>{Callvote.Instance.Translation.NoSuccessFullRestart
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdRestartRound%", Callvote.Instance.Config.ThresholdRestartRound.ToString())}</size>");
            }
        }

        public static Dictionary<string, string> AddOptions()
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            return VotingHandler.Options;
        }

        private static string ReplacePlayer(Player player)
        {
            return Callvote.Instance.Translation.AskedToRestart.Replace("%Player%", player.Nickname);
        }
    }
}
