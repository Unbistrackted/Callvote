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
    public class RestartRoundCommand : ICommand
    {
        public string Command => "restartround";

        public string[] Aliases => new string[] { "restart", "rround" };

        public string Description => "Calls a voting for restarting the round";
        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get((CommandSender)sender);

            if (!Plugin.Instance.Config.EnableRoundRestart || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.VoteRestartRoundDisabled;
                return true;
            }

            if (!player.CheckPermission("cv.callvoterestartround") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return true;
            }

            if (Plugin.Instance.Roundtimer < Plugin.Instance.Config.MaxWaitRestartRound || !player.CheckPermission("cv.bypass"))
            {

                response = Plugin.Instance.Translation.WaitToVote
                    .Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitRestartRound - Plugin.Instance.Roundtimer}");
                return true;
            }


            options.Add("yes", Plugin.Instance.Translation.OptionYes);
            options.Add("no", Plugin.Instance.Translation.OptionNo);

            VoteHandler.StartVote(Plugin.Instance.Translation.AskedToRestart.Replace("%Player%", player.Nickname), options,
            delegate (Vote vote)
            {
                int yesVotePercent = (int)(vote.Counter["yes"] / (float)(Player.List.Count()) * 100f);
                int noVotePercent = (int)(vote.Counter["no"] / (float)(Player.List.Count()) * 100f);
                if (yesVotePercent >= Plugin.Instance.Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
                {
                    Map.Broadcast(5, Plugin.Instance.Translation.RoundRestarting.Replace("%VotePercent%", yesVotePercent.ToString()));
                    Round.Restart();

                }
                else
                {
                    Map.Broadcast(5, Plugin.Instance.Translation.NoSuccessFullRestart.Replace("%VotePercent%", noVotePercent.ToString()).Replace("%ThresholdRestartRound%", Plugin.Instance.Config.ThresholdRestartRound.ToString()));
                }
            });
            response = "Vote started.";
            return true;
        }
    }

}
