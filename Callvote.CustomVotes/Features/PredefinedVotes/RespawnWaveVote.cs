using LabApi.Features.Wrappers;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.Features.Displays;
using Callvote.API.Features.Votes;
using Callvote.API.Interfaces;
using Callvote.Features.VoteTemplate;
using LabApi.Features.Console;
using PlayerRoles;
using Respawning;
using Respawning.Config;
using Respawning.Waves;

namespace Callvote.CustomVotes.Features.PredefinedVotes
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
            : base(player, ReplacePlayer(player), nameof(CustomVoteType.RespawnWave), AddOptions(out VoteOption no, out VoteOption mtf, out VoteOption ci), AddCallback)
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

            if (mtfVotePercent >= Plugin.Instance.Config.ThresholdRespawnWave)
            {
                message = Plugin.Instance.Translation.MtfRespawn
                    .Replace("%VotePercent%", mtfVotePercent + "%")
                    .Replace("%VoteDetail%", respawnVote.MtfVoteOption.Detail);

                SpawnableWaveBase mtfWave = GetWaveBase(Faction.FoundationStaff);

                if (mtfWave == null)
                {
                    Logger.Warn($"NW Moment!!!!! For some reason Wave is null!!!!!!!! I LOVE NW!!!!!!");
                    return;
                }

                WaveManager.Spawn(mtfWave);
            }
            else if (ciVotePercent >= Plugin.Instance.Config.ThresholdRespawnWave)
            {
                message = Plugin.Instance.Translation.CiRespawn
                    .Replace("%VotePercent%", ciVotePercent + "%")
                    .Replace("%VoteDetail%", respawnVote.CiVoteOption.Detail);

                SpawnableWaveBase ciWave = GetWaveBase(Faction.FoundationEnemy);

                if (ciWave == null)
                {
                    Logger.Warn($"NW Moment!!!!! For some reason Wave is null!!!!!!!! I LOVE NW!!!!!!");
                    return;
                }

                WaveManager.Spawn(ciWave);
            }
            else
            {
                message = Plugin.Instance.Translation.NoSuccessFullRespawn
                    .Replace("%VotePercent%", noVotePercent.ToString())
                    .Replace("%ThresholdRespawnWave%", Plugin.Instance.Config.ThresholdRespawnWave.ToString())
                    .Replace("%VoteDetail%", respawnVote.NoVoteOption.Detail);
            }

            DisplayHandler.Show(CallvotePlugin.Instance.Config.FinalResultsDuration, message, vote.AllowedPlayers);
        }

        private static HashSet<VoteOption> AddOptions(out VoteOption no, out VoteOption mtf, out VoteOption ci)
        {
            no = new VoteOption(CallvotePlugin.Instance.Translation.CommandNo, CallvotePlugin.Instance.Translation.DetailNo);
            mtf = new VoteOption(Plugin.Instance.Translation.CommandMobileTaskForce, Plugin.Instance.Translation.DetailMtf);
            ci = new VoteOption(Plugin.Instance.Translation.CommandChaosInsurgency, Plugin.Instance.Translation.DetailCi);

            return [no, mtf, ci];
        }

        private static string ReplacePlayer(Player player) => Plugin.Instance.Translation.AskedToRespawn.Replace("%Player%", player.Nickname);
        private static SpawnableWaveBase GetWaveBase(Faction faction) => WaveManager.Waves.Where(wave => wave.TargetFaction == faction && wave.Configuration.GetType().GetGenericTypeDefinition() == typeof(PrimaryWaveConfig<>)).FirstOrDefault();
    }
}
