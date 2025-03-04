using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    public class RestartRoundCommand : ICommand
    {
        public string Command => "restartround";

        public string[] Aliases => new[] { "restart", "rround" };

        public string Description => "Calls a restart round voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get(sender);

            if (!Plugin.Instance.Config.EnableRoundRestart)
            {
                response = Plugin.Instance.Translation.VoteRestartRoundDisabled;
                return false;
            }

            if (!player.CheckPermission("cv.callvoterestartround"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Plugin.Instance.Config.MaxWaitRestartRound || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitRestartRound - Round.ElapsedTime.TotalSeconds}");
                return false;
            }


            options.Add(Plugin.Instance.Translation.CommandYes, Plugin.Instance.Translation.OptionYes);
            options.Add(Plugin.Instance.Translation.CommandNo, Plugin.Instance.Translation.OptionNo);

            VoteAPI.CurrentVoting = new Voting(Plugin.Instance.Translation.AskedToRestart
                .Replace("%Player%", player.Nickname),
                options,
                delegate(Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                    if (yesVotePercent >= Plugin.Instance.Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
                    {
                        Map.Broadcast(5, Plugin.Instance.Translation.RoundRestarting
                            .Replace("%VotePercent%", yesVotePercent.ToString()));
                        Round.Restart();
                    }
                    else
                    {
                        Map.Broadcast(5, Plugin.Instance.Translation.NoSuccessFullRestart
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%ThresholdRestartRound%", Plugin.Instance.Config.ThresholdRestartRound.ToString()));
                    }
                });
            response = VoteAPI.CurrentVoting.Response;
            return true;
        }
    }
}