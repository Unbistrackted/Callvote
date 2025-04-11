using Callvote.Enums;
using Callvote.Features;
using Callvote.Interfaces;
using Exiled.API.Features;
using Respawning;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.API.VotingsTemplate
{
    public class RespawnWaveVoting : Voting, IVotingTemplate
    {
        public RespawnWaveVoting(Player player) : base(
            ReplacePlayer(player),
            nameof(VotingTypeEnum.RespawnWave),
            player,
            vote =>
            {
                int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                int mtfVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandMobileTaskForce] / (float)Player.List.Count() * 100f);
                int ciVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandChaosInsurgency] / (float)Player.List.Count() * 100f);
                if (mtfVotePercent >= Callvote.Instance.Config.ThresholdRespawnWave)
                {
                    Map.Broadcast(5, Callvote.Instance.Translation.MtfRespawn
                        .Replace("%VotePercent%", mtfVotePercent + "%"));
                    WaveManager.Spawn(WaveManager.Waves[0]);
                }
                else if (ciVotePercent >= Callvote.Instance.Config.ThresholdRespawnWave)
                {
                    Map.Broadcast(5, Callvote.Instance.Translation.CiRespawn
                        .Replace("%VotePercent%", ciVotePercent.ToString()));
                    WaveManager.Spawn(WaveManager.Waves[1]);
                }
                else
                {
                    Map.Broadcast(5, Callvote.Instance.Translation.NoSuccessFullRespawn
                        .Replace("%VotePercent%", noVotePercent.ToString())
                        .Replace("%ThresholdRespawnWave%", Callvote.Instance.Config.ThresholdRespawnWave.ToString()));
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
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandMobileTaskForce, Callvote.Instance.Translation.OptionMtf);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandChaosInsurgency, Callvote.Instance.Translation.OptionCi);
            return VotingHandler.Options;
        }
    }
}
