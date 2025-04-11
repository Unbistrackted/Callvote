using Callvote.Enums;
using Callvote.Features;
using Callvote.Interfaces;
using Exiled.API.Features;
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
            vote =>
            {
                int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                if (yesVotePercent >= Callvote.Instance.Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
                {
                    Map.Broadcast(5, Callvote.Instance.Translation.RoundRestarting
                        .Replace("%VotePercent%", yesVotePercent.ToString()));
                    Round.Restart();
                }
                else
                {
                    Map.Broadcast(5, Callvote.Instance.Translation.NoSuccessFullRestart
                        .Replace("%VotePercent%", yesVotePercent.ToString())
                        .Replace("%ThresholdRestartRound%", Callvote.Instance.Config.ThresholdRestartRound.ToString()));
                }
            },
            AddOptions())
        {
        }

        private static string ReplacePlayer(Player player)
        {
            return Callvote.Instance.Translation.AskedToRespawn
                    .Replace("%Player%", player.Nickname);
        }
        private static Dictionary<string, string> AddOptions()
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            return VotingHandler.Options;
        }
    }
}
