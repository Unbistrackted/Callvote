using Callvote.Enums;
using Callvote.Features;
using Callvote.Interfaces;
using LabApi.Features.Wrappers;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.API.VotingsTemplate
{
    public class RespawnWaveVoting : Voting, IVotingTemplate
    {
        public RespawnWaveVoting(Player player) : base(ReplacePlayer(player), nameof(VotingTypeEnum.RespawnWave), player, AddCallback, AddOptions()) { }

        public static void AddCallback(Voting vote)
        {
            int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
            int mtfVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandMobileTaskForce] / (float)Player.List.Count() * 100f);
            int ciVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandChaosInsurgency] / (float)Player.List.Count() * 100f);
            if (mtfVotePercent >= Callvote.Instance.Config.ThresholdRespawnWave)
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.MtfRespawn)}>{Callvote.Instance.Translation.MtfRespawn
                    .Replace("%VotePercent%", mtfVotePercent + "%")}</size>");
                WaveManager.Spawn(WaveManager.Waves[0]);
            }
            else if (ciVotePercent >= Callvote.Instance.Config.ThresholdRespawnWave)
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.CiRespawn)}>{Callvote.Instance.Translation.CiRespawn
                    .Replace("%VotePercent%", ciVotePercent.ToString())}</size>");
                WaveManager.Spawn(WaveManager.Waves[1]);
            }
            else
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.NoSuccessFullRespawn)}>{Callvote.Instance.Translation.NoSuccessFullRespawn
                    .Replace("%VotePercent%", noVotePercent.ToString())
                    .Replace("%ThresholdRespawnWave%", Callvote.Instance.Config.ThresholdRespawnWave.ToString())}</size>");
            }
        }

        public static Dictionary<string, string> AddOptions()
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandMobileTaskForce, Callvote.Instance.Translation.OptionMtf);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandChaosInsurgency, Callvote.Instance.Translation.OptionCi);
            return VotingHandler.Options;
        }

        private static string ReplacePlayer(Player player)
        {
            return Callvote.Instance.Translation.AskedToRespawn.Replace("%Player%", player.Nickname);
        }
    }
}
