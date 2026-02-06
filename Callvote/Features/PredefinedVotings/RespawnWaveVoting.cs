#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API;
using Callvote.API.VotingsTemplate;
using Callvote.Features.Enums;
using Callvote.Features.Interfaces;
using Respawning;

namespace Callvote.Features.PredefinedVotings
{
    /// <summary>
    /// Represents the type for the Respawn Wave Predefined Voting.
    /// Initializes a new instance of the <see cref="RespawnWaveVoting"/> class.
    /// </summary>
    /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
    public class RespawnWaveVoting(Player player) : CustomVoting(player, ReplacePlayer(player), nameof(VotingTypeEnum.RespawnWave), AddCallback, AddOptions()), IVotingTemplate
    {
        private static void AddCallback(Voting vote)
        {
            int noVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
            int mtfVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandMobileTaskForce] / (float)Player.List.Count() * 100f);
            int ciVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandChaosInsurgency] / (float)Player.List.Count() * 100f);

            if (mtfVotePercent >= CallvotePlugin.Instance.Config.ThresholdRespawnWave)
            {
                SoftDependency.MessageProvider.DisplayMessage(
                    TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                    $"<size={DisplayMessageHelper.CalculateMessageSize(CallvotePlugin.Instance.Translation.MtfRespawn)}>{CallvotePlugin.Instance.Translation.MtfRespawn
                    .Replace("%VotePercent%", mtfVotePercent + "%")}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
                WaveManager.Spawn(WaveManager.Waves[0]);
            }
            else if (ciVotePercent >= CallvotePlugin.Instance.Config.ThresholdRespawnWave)
            {
                SoftDependency.MessageProvider.DisplayMessage(
                    TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                    $"<size={DisplayMessageHelper.CalculateMessageSize(CallvotePlugin.Instance.Translation.CiRespawn)}>{CallvotePlugin.Instance.Translation.CiRespawn
                    .Replace("%VotePercent%", ciVotePercent.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
                WaveManager.Spawn(WaveManager.Waves[1]);
            }
            else
            {
                SoftDependency.MessageProvider.DisplayMessage(
                    TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                    $"<size={DisplayMessageHelper.CalculateMessageSize(CallvotePlugin.Instance.Translation.NoSuccessFullRespawn)}>{CallvotePlugin.Instance.Translation.NoSuccessFullRespawn
                    .Replace("%VotePercent%", noVotePercent.ToString())
                    .Replace("%ThresholdRespawnWave%", CallvotePlugin.Instance.Config.ThresholdRespawnWave.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
        }

        private static Dictionary<string, string> AddOptions()
        {
            Dictionary<string, string> options = [];
            options.Add(CallvotePlugin.Instance.Translation.CommandNo, CallvotePlugin.Instance.Translation.OptionNo);
            options.Add(CallvotePlugin.Instance.Translation.CommandMobileTaskForce, CallvotePlugin.Instance.Translation.OptionMtf);
            options.Add(CallvotePlugin.Instance.Translation.CommandChaosInsurgency, CallvotePlugin.Instance.Translation.OptionCi);

            return options;
        }

        private static string ReplacePlayer(Player player)
        {
            return CallvotePlugin.Instance.Translation.AskedToRespawn.Replace("%Player%", player.Nickname);
        }
    }
}
