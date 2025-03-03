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
    public class NukeCommand : ICommand
    {
        public string Command => "nuke";

        public string[] Aliases => new string[] { "nu", "bomba" };

        public string Description => "";
        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get((CommandSender)sender);

            if (!player.CheckPermission("cv.callvotenuke") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return true;
            }

            if (Plugin.Instance.Roundtimer < Plugin.Instance.Config.MaxWaitNuke || !player.CheckPermission("cv.bypass"))
            {

                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitKick - Plugin.Instance.Roundtimer}");
                return true;
            }

            if (!Plugin.Instance.Config.EnableNuke || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.VoteNukeDisabled;
                return true;
            }


            options.Add("yes", Plugin.Instance.Translation.OptionYes);
            options.Add("no", Plugin.Instance.Translation.OptionNo);

            VoteHandler.StartVote(Plugin.Instance.Translation.AskedToNuke.Replace("%Player%", player.Nickname), options,
            delegate (Vote vote)
            {

                int yesVotePercent = (int)(vote.Counter["yes"] / (float)(Player.List.Count()) * 100f);
                int noVotePercent = (int)(vote.Counter["no"] / (float)(Player.List.Count()) * 100f); 
                if (yesVotePercent >= Plugin.Instance.Config.ThresholdNuke && yesVotePercent > noVotePercent)
                {
                    Map.Broadcast(5, Plugin.Instance.Translation.FoundationNuked
                        .Replace("%VotePercent%", yesVotePercent.ToString()));
                    Exiled.API.Features.Warhead.Start();

                }
                else
                {
                    Map.Broadcast(5, Plugin.Instance.Translation.NoSuccessFullNuke.Replace("%VotePercent%", yesVotePercent.ToString()).Replace("%ThresholdNuke%", Plugin.Instance.Config.ThresholdNuke.ToString()));
                }

            });
            response = "Vote started.";
            return true;
        }
    }

}
