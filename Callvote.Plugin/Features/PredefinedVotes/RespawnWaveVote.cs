#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.Enums;
using Callvote.API.Features.Display;
using Callvote.API.Features.Votes;
using Callvote.API.Interfaces;
using Callvote.Features.VoteTemplate;
using PlayerRoles;
using Respawning;
using Respawning.Config;
using Respawning.Waves;

namespace Callvote.Features.PredefinedVotes
{
    /// <summary>
    /// Represents the type for the Respawn Wave Predefined Vote.
    /// </summary>
    public class RespawnWaveVote : CustomVote, IPredefinedVote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RespawnWaveVote"/> class.
        /// </summary>
        /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
        public RespawnWaveVote(Player player)
            : base(player, ReplacePlayer(player), nameof(VoteType.RespawnWave), AddOptions(out VoteOption no, out VoteOption mtf, out VoteOption ci), AddCallback)
        {
            this.NoVoteOption = no;
            this.MtfVoteOption = mtf;
            this.CiVoteOption = ci;
        }

        /// <summary>
        /// Gets the No <see cref="VoteOption"/> option.
        /// </summary>
        public VoteOption NoVoteOption { get; }

        /// <summary>
        /// Gets the Mtf <see cref="VoteOption"/> option.
        /// </summary>
        public VoteOption MtfVoteOption { get; }

        /// <summary>
        /// Gets the Ci <see cref="VoteOption"/> option.
        /// </summary>
        public VoteOption CiVoteOption { get; }

        private static void AddCallback(Vote vote)
        {
            if (vote is not RespawnWaveVote respawnVote)
            {
                return;
            }

            int noVotePercent = vote.GetVoteOptionPercentage(respawnVote.NoVoteOption);
            int mtfVotePercent = vote.GetVoteOptionPercentage(respawnVote.MtfVoteOption);
            int ciVotePercent = vote.GetVoteOptionPercentage(respawnVote.CiVoteOption);

            string message;

            if (mtfVotePercent >= CallvotePlugin.Instance.Config.ThresholdRespawnWave)
            {
                message = CallvotePlugin.Instance.Translation.MtfRespawn
                    .Replace("%VotePercent%", mtfVotePercent + "%")
                    .Replace("%VoteDetail%", respawnVote.MtfVoteOption.Detail);

                SpawnableWaveBase mtfWave = WaveManager.Waves.Where(wave => wave.TargetFaction == Faction.FoundationStaff && wave.Configuration is not StandardWaveConfig<NtfMiniWave>).FirstOrDefault();

                if (mtfWave == null)
                {
                    ServerConsole.AddLog($"[ERROR] [Callvote] NW Moment!!!!! For some reason Wave is null!!!!!!!! I LOVE NW!!!!!!", ConsoleColor.Red);
                    return;
                }

                WaveManager.Spawn(mtfWave);
            }
            else if (ciVotePercent >= CallvotePlugin.Instance.Config.ThresholdRespawnWave)
            {
                message = CallvotePlugin.Instance.Translation.CiRespawn
                    .Replace("%VotePercent%", ciVotePercent + "%")
                    .Replace("%VoteDetail%", respawnVote.CiVoteOption.Detail);

                SpawnableWaveBase ciWave = WaveManager.Waves.Where(wave => wave.TargetFaction == Faction.FoundationEnemy && wave.Configuration is not StandardWaveConfig<ChaosMiniWave>).FirstOrDefault();

                if (ciWave == null)
                {
                    ServerConsole.AddLog($"[ERROR] [Callvote] NW Moment!!!!! For some reason Wave is null!!!!!!!! I LOVE NW!!!!!!", ConsoleColor.Red);
                    return;
                }

                WaveManager.Spawn(ciWave);
            }
            else
            {
                message = CallvotePlugin.Instance.Translation.NoSuccessFullRespawn
                    .Replace("%VotePercent%", noVotePercent.ToString())
                    .Replace("%ThresholdRespawnWave%", CallvotePlugin.Instance.Config.ThresholdRespawnWave.ToString())
                    .Replace("%VoteDetail%", respawnVote.NoVoteOption.Detail);
            }

            DisplayHandler.Show(CallvotePlugin.Instance.Config.FinalResultsDuration, message, vote.AllowedPlayers);
        }

        private static HashSet<VoteOption> AddOptions(out VoteOption no, out VoteOption mtf, out VoteOption ci)
        {
            no = new VoteOption(CallvotePlugin.Instance.Translation.CommandNo, CallvotePlugin.Instance.Translation.DetailNo);
            mtf = new VoteOption(CallvotePlugin.Instance.Translation.CommandMobileTaskForce, CallvotePlugin.Instance.Translation.DetailMtf);
            ci = new VoteOption(CallvotePlugin.Instance.Translation.CommandChaosInsurgency, CallvotePlugin.Instance.Translation.DetailCi);

            return [no, mtf, ci];
        }

        private static string ReplacePlayer(Player player)
        {
            return CallvotePlugin.Instance.Translation.AskedToRespawn.Replace("%Player%", player.Nickname);
        }
    }
}
