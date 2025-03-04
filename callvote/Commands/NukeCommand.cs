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

        public string Description => "Calls a nuke voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get(sender);

            if (!Plugin.Instance.Config.EnableNuke)
            {
                response = Plugin.Instance.Translation.VoteNukeDisabled;
                return false;
            }

            if (!player.CheckPermission("cv.callvotenuke"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Plugin.Instance.Config.MaxWaitNuke || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitNuke - Round.ElapsedTime.TotalSeconds}");
                return false;
            }

            options.Add(Plugin.Instance.Translation.CommandYes, Plugin.Instance.Translation.OptionYes);
            options.Add(Plugin.Instance.Translation.CommandNo, Plugin.Instance.Translation.OptionNo);

            VotingAPI.CurrentVoting = new Voting(Plugin.Instance.Translation.AskedToNuke
                .Replace("%Player%", player.Nickname), 
                options,
                player,
                delegate(Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
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
            response = VotingAPI.CurrentVoting.Response;
            return true;
        }
    }
}