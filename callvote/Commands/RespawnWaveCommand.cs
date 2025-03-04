using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Respawning;

namespace Callvote.Commands
{
    public class RespawnWaveCommand : ICommand
    {
        public string Command => "respawnwave";

        public string[] Aliases => new[] { "respawn", "wave" };

        public string Description => "Calls a respawn wave voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get(sender);

            if (!Plugin.Instance.Config.EnableRespawnWave)
            {
                response = Plugin.Instance.Translation.VoteRespawnWaveDisabled;
                return false;
            }

            if (!player.CheckPermission("cv.callvoterespawnwave"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Plugin.Instance.Config.MaxWaitRespawnWave || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitRespawnWave - Round.ElapsedTime.TotalSeconds}");
                return false;
            }

            options.Add(Plugin.Instance.Translation.CommandNo, Plugin.Instance.Translation.OptionNo);
            options.Add(Plugin.Instance.Translation.CommandMobileTaskForce, Plugin.Instance.Translation.OptionMtf);
            options.Add(Plugin.Instance.Translation.CommandChaosInsurgency, Plugin.Instance.Translation.OptionCi);

            VoteAPI.CurrentVoting = new Voting(Plugin.Instance.Translation.AskedToRespawn
                .Replace("%Player%", player.Nickname),
                options,
                delegate(Voting vote)
                {
                    int noVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                    int mtfVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandMobileTaskForce] / (float)Player.List.Count() * 100f);
                    int ciVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandChaosInsurgency] / (float)Player.List.Count() * 100f);
                    if (mtfVotePercent >= Plugin.Instance.Config.ThresholdRespawnWave)
                    {
                        Map.Broadcast(5, Plugin.Instance.Translation.MtfRespawn
                            .Replace("%VotePercent%", mtfVotePercent + "%"));
                        WaveManager.Spawn(WaveManager.Waves[0]);
                    }
                    else if (ciVotePercent >= Plugin.Instance.Config.ThresholdRespawnWave)
                    {
                        Map.Broadcast(5, Plugin.Instance.Translation.CiRespawn
                            .Replace("%VotePercent%", ciVotePercent.ToString()));
                        WaveManager.Spawn(WaveManager.Waves[1]);
                    }
                    else
                    {
                        Map.Broadcast(5, Plugin.Instance.Translation.NoSuccessFullRespawn
                            .Replace("%VotePercent%", noVotePercent.ToString())
                            .Replace("%ThresholdRespawnWave%", Plugin.Instance.Config.ThresholdRespawnWave.ToString()));
                    }
                });
            response = VoteAPI.CurrentVoting.Response;
            return true;
        }
    }
}