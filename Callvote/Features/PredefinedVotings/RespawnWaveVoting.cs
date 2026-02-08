#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.VotingsTemplate;
using Callvote.Configuration;
using Callvote.Features.Enums;
using Callvote.Features.Interfaces;
using PlayerRoles;
using Respawning;
using Respawning.Config;
using Respawning.Waves;

namespace Callvote.Features.PredefinedVotings
{
    /// <summary>
    /// Represents the type for the Respawn Wave Predefined Voting.
    /// </summary>
    public class RespawnWaveVoting : CustomVoting, IVotingTemplate
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        /// <summary>
        /// Initializes a new instance of the <see cref="RespawnWaveVoting"/> class.
        /// </summary>
        /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
        public RespawnWaveVoting(Player player)
            : base(player, ReplacePlayer(player), nameof(VotingTypeEnum.RespawnWave), AddCallback, AddOptions(out Vote no, out Vote mtf, out Vote ci))
        {
            NoVote = no;
            MtfVote = mtf;
            CiVote = ci;
        }

        /// <summary>
        /// Gets the No <see cref="Vote"/> option.
        /// </summary>
        public Vote NoVote { get; }

        /// <summary>
        /// Gets the Mtf <see cref="Vote"/> option.
        /// </summary>
        public Vote MtfVote { get; }

        /// <summary>
        /// Gets the Ci <see cref="Vote"/> option.
        /// </summary>
        public Vote CiVote { get; }

        private static void AddCallback(Voting voting)
        {
            if (voting is not RespawnWaveVoting respawnVoting)
            {
                return;
            }

            int noVotePercent = voting.GetVotePercentage(respawnVoting.NoVote);
            int mtfVotePercent = voting.GetVotePercentage(respawnVoting.MtfVote);
            int ciVotePercent = voting.GetVotePercentage(respawnVoting.CiVote);

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
                voting.AllowedPlayers);
        }

        private static HashSet<Vote> AddOptions(out Vote no, out Vote mtf, out Vote ci)
        {
            no = new Vote(Translation.CommandNo, Translation.OptionNo);
            mtf = new Vote(Translation.CommandMobileTaskForce, Translation.OptionMtf);
            ci = new Vote(Translation.CommandChaosInsurgency, Translation.OptionCi);

            return [no, mtf, ci];
        }

        private static string ReplacePlayer(Player player)
        {
            return Translation.AskedToRespawn.Replace("%Player%", player.Nickname);
        }
    }
}
