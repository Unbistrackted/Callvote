using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using UnityEngine;
using System.Text.RegularExpressions;
using callvote.VoteHandlers;
using MEC;

namespace callvote.Commands
{
    public class RespawnWaveCommand : ICommand
    {
        public string Command => "respawnwave";

        public string[] Aliases => new string[] { "respawn", "wave" };

        public string Description => "";
        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get((CommandSender)sender);

            if (!Plugin.Instance.Config.EnableRespawnWave || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.VoteNukeDisabled;
                return true;
            }

            if (!player.CheckPermission("cv.callvoterespawnwave") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return true;
            }

            if (Plugin.Instance.Roundtimer < Plugin.Instance.Config.MaxWaitRespawnWave || !player.CheckPermission("cv.bypass"))
            {

                response = Plugin.Instance.Translation.WaitToVote
                    .Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitRespawnWave- Plugin.Instance.Roundtimer}");
                return true;
            }

            
            options.Add("no", Plugin.Instance.Translation.OptionNo);
            options.Add("ci", Plugin.Instance.Translation.OptionMTF);
            options.Add("mtf", Plugin.Instance.Translation.OptionCi);

            VoteHandler.StartVote(Plugin.Instance.Translation.AskedToRespawn.Replace("%Player%", player.Nickname), options,
            delegate (Vote vote)
            {
                int noVotePercent = (int)(vote.Counter["no"] / (float)(Player.List.Count()) * 100f);
                int mtfVotePercent = (int)((float)vote.Counter["mtf"] / (float)(Player.List.Count()) * 100f);
                int ciVotePercent = (int)((float)vote.Counter["ci"] / (float)(Player.List.Count()) * 100f);
                if (mtfVotePercent >= Plugin.Instance.Config.ThresholdRespawnWave)
                {
                    Map.Broadcast(5, Plugin.Instance.Translation.mtfRespawn.Replace("%VotePercent%", mtfVotePercent.ToString() + "%"));
                    Respawning.WaveManager.Spawn(Respawning.WaveManager.Waves[0]); //Gets MTF wave from List, [index] following implementation

                }
                else if (ciVotePercent >= Plugin.Instance.Config.ThresholdRespawnWave)
                {
                    Map.Broadcast(5, Plugin.Instance.Translation.ciRespawn.Replace("%VotePercent%", ciVotePercent.ToString()));
                    Respawning.WaveManager.Spawn(Respawning.WaveManager.Waves[1]); //Gets CI wave from List, [index] following implementation
                }
                else
                {
                    Map.Broadcast(5, Plugin.Instance.Translation.NoSuccessFullRespawn.Replace("%VotePercent%", noVotePercent.ToString()).Replace("%ThresholdRespawnWave%", Plugin.Instance.Config.ThresholdRespawnWave.ToString()));
                }
            });
            response = "Vote started.";
            return true;
        }
    }

}
