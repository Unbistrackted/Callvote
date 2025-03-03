using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    public class KickCommand : ICommand
    {
        public string Command => "kick";

        public string[] Aliases => new[] { "ki", "k" };

        public string Description => "";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            var options = new Dictionary<string, string>();

            var player = Player.Get((CommandSender)sender);


            if (!player.CheckPermission("cv.callvotekick") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return true;
            }

            if (args.Count == 0)
            {
                response = "callvote Kick <player> (reason)";
                return true;
            }

            if (args.Count == 1)
            {
                response = Plugin.Instance.Translation.PassReason;
                return true;
            }

            if (Round.ElapsedTime.TotalSeconds < Plugin.Instance.Config.MaxWaitRestartRound ||
                !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%",
                    $"{Plugin.Instance.Config.MaxWaitKick - Round.ElapsedTime.TotalSeconds}");
                return true;
            }

            if (!Plugin.Instance.Config.EnableKick || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.VoteKickDisabled;
                return true;
            }

            var locatedPlayer = Player.Get(args.ElementAt(0));

            if (locatedPlayer == null)
            {
                response = Plugin.Instance.Translation.PlayerNotFound.Replace("%Player%", args.ElementAt(0));
                return true;
            }


            var playerSearch = Player.List.Where(p => p.Nickname.Contains(args.ElementAt(0))).ToList();
            if (playerSearch.Count() < 0 || playerSearch.Count() > 1)
            {
                response = Plugin.Instance.Translation.PlayersWithSameName.Replace("%Player%", args.ElementAt(0));
                return true;
            }


            var reason = args.ElementAt(1);

            options.Add(Plugin.Instance.Translation.CommandYes, Plugin.Instance.Translation.OptionYes);
            options.Add(Plugin.Instance.Translation.CommandNo, Plugin.Instance.Translation.OptionNo);

            VoteAPI.StartVote(
                Plugin.Instance.Translation.AskedToKick.Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", locatedPlayer.Nickname).Replace("%Reason%", reason), options,
                delegate(Vote vote)
                {
                    var yesVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandYes] /
                        (float)Player.List.Count() * 100f);
                    var noVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandNo] /
                        (float)Player.List.Count() * 100f); //Just so you know that it exists
                    if (yesVotePercent >= Plugin.Instance.Config.ThresholdKick && yesVotePercent > noVotePercent)
                    {
                        Map.Broadcast(8, Plugin.Instance.Translation.PlayerGettingKicked
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%Player%", player.Nickname)
                            .Replace("%Offender%", locatedPlayer.Nickname)
                            .Replace("%Reason%", reason));

                        if (!locatedPlayer.CheckPermission("cv.untouchable"))
                            locatedPlayer.Kick(Plugin.Instance.Translation.Untouchable
                                .Replace("%VotePercent%", yesVotePercent.ToString()));
                    }
                    else
                    {
                        Map.Broadcast(5, Plugin.Instance.Translation.NotSuccessFullKick
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%ThresholdKick%", Plugin.Instance.Config.ThresholdKick.ToString())
                            .Replace("%Offender%", locatedPlayer.Nickname));
                    }
                });
            response = Plugin.Instance.Translation.VoteStarted;
            return true;
        }
    }
}