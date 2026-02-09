#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.VoteTemplate;
using Callvote.Configuration;
using Callvote.Features.Enums;
using Callvote.Features.Interfaces;
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
            : base(player, ReplacePlayer(player), nameof(VoteTypeEnum.RespawnWave), AddCallback, AddOptions(out VoteOption no, out VoteOption mtf, out VoteOption ci))
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

        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

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

            if (mtfVotePercent >= Config.ThresholdRespawnWave)
            {
                message = Translation.MtfRespawn.Replace("%VotePercent%", mtfVotePercent + "%");

                SpawnableWaveBase mtfWave = WaveManager.Waves.Where(wave => wave.TargetFaction == Faction.FoundationStaff && wave.Configuration is not StandardWaveConfig<NtfMiniWave>).FirstOrDefault();

                if (mtfWave == null)
                {
                    ServerConsole.AddLog($"[ERROR] [Callvote] NW Moment!!!!! For some reason Wave is null!!!!!!!! I LOVE NW!!!!!!", ConsoleColor.Red);
                    return;
                }

                WaveManager.Spawn(mtfWave);
            }
            else if (ciVotePercent >= Config.ThresholdRespawnWave)
            {
                message = Translation.CiRespawn.Replace("%VotePercent%", ciVotePercent + "%");

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
                message = Translation.NoSuccessFullRespawn
                    .Replace("%VotePercent%", noVotePercent.ToString())
                    .Replace("%ThresholdRespawnWave%", Config.ThresholdRespawnWave.ToString());
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.FinalResultsDuration),
                $"<size={DisplayMessageHelper.CalculateMessageSize(message)}>{message}</size>",
                vote.AllowedPlayers);
        }

        private static HashSet<VoteOption> AddOptions(out VoteOption no, out VoteOption mtf, out VoteOption ci)
        {
            no = new VoteOption(Translation.CommandNo, Translation.DetailNo);
            mtf = new VoteOption(Translation.CommandMobileTaskForce, Translation.DetailMtf);
            ci = new VoteOption(Translation.CommandChaosInsurgency, Translation.DetailCi);

            return [no, mtf, ci];
        }

        private static string ReplacePlayer(Player player)
        {
            return Translation.AskedToRespawn.Replace("%Player%", player.Nickname);
        }
    }
}
