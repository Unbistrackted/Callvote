using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    public class NukeCommand : ICommand
    {
        public string Command => "nuke";

        public string[] Aliases => new[] { "nu", "bomba" };

        public string Description => "";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            var options = new Dictionary<string, string>();

            var player = Player.Get((CommandSender)sender);

            if (!player.CheckPermission("cv.callvotenuke"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return true;
            }

            if (Round.ElapsedTime.TotalSeconds < Plugin.Instance.Config.MaxWaitNuke || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%",
                    $"{Plugin.Instance.Config.MaxWaitKick - Round.ElapsedTime.TotalSeconds}");
                return true;
            }

            if (!Plugin.Instance.Config.EnableNuke)
            {
                response = Plugin.Instance.Translation.VoteNukeDisabled;
                return true;
            }


            options.Add(Plugin.Instance.Translation.CommandYes, Plugin.Instance.Translation.OptionYes);
            options.Add(Plugin.Instance.Translation.CommandNo, Plugin.Instance.Translation.OptionNo);

            VoteAPI.StartVote(Plugin.Instance.Translation.AskedToNuke.Replace("%Player%", player.Nickname), options,
                delegate(Vote vote)
                {
                    var yesVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandYes] /
                        (float)Player.List.Count() * 100f);
                    var noVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandNo] /
                        (float)Player.List.Count() * 100f);
                    if (yesVotePercent >= Plugin.Instance.Config.ThresholdNuke && yesVotePercent > noVotePercent)
                    {
                        Map.Broadcast(5, Plugin.Instance.Translation.FoundationNuked
                            .Replace("%VotePercent%", yesVotePercent.ToString()));
                        Warhead.Start();
                    }
                    else
                    {
                        Map.Broadcast(5, Plugin.Instance.Translation.NoSuccessFullNuke
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%ThresholdNuke%", Plugin.Instance.Config.ThresholdNuke.ToString()));
                    }
                });
            response = Plugin.Instance.Translation.VoteStarted;
            return true;
        }
    }
}