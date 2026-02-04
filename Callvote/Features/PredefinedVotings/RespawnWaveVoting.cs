#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.Features.Enums;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.VotingsTemplate;
using Callvote.Features.Interfaces;
using Callvote.API;

namespace Callvote.Features.PredefinedVotings
{
    public class RespawnWaveVoting(Player player) : CustomVoting(player, ReplacePlayer(player), nameof(VotingTypeEnum.RespawnWave), AddCallback, AddOptions()), IVotingTemplate
    {
        public static void AddCallback(Voting vote)
        {
            int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
            int mtfVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandMobileTaskForce] / (float)Player.List.Count() * 100f);
            int ciVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandChaosInsurgency] / (float)Player.List.Count() * 100f);

            if (mtfVotePercent >= Callvote.Instance.Config.ThresholdRespawnWave)
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.MtfRespawn)}>{Callvote.Instance.Translation.MtfRespawn
                    .Replace("%VotePercent%", mtfVotePercent + "%")}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
                WaveManager.Spawn(WaveManager.Waves[0]);
            }
            else if (ciVotePercent >= Callvote.Instance.Config.ThresholdRespawnWave)
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.CiRespawn)}>{Callvote.Instance.Translation.CiRespawn
                    .Replace("%VotePercent%", ciVotePercent.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
                WaveManager.Spawn(WaveManager.Waves[1]);
            }
            else
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.NoSuccessFullRespawn)}>{Callvote.Instance.Translation.NoSuccessFullRespawn
                    .Replace("%VotePercent%", noVotePercent.ToString())
                    .Replace("%ThresholdRespawnWave%", Callvote.Instance.Config.ThresholdRespawnWave.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
        }

        public static Dictionary<string, string> AddOptions()
        {
            Dictionary<string, string> options = [];
            options.Add(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            options.Add(Callvote.Instance.Translation.CommandMobileTaskForce, Callvote.Instance.Translation.OptionMtf);
            options.Add(Callvote.Instance.Translation.CommandChaosInsurgency, Callvote.Instance.Translation.OptionCi);

            return options;
        }

        private static string ReplacePlayer(Player player)
        {
            return Callvote.Instance.Translation.AskedToRespawn.Replace("%Player%", player.Nickname);
        }
    }
}
