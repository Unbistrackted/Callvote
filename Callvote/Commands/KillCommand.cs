using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    public class KillCommand : ICommand
    {
        public string Command => "kill";

        public string[] Aliases => new[] { "death", "kil" };

        public string Description => "Calls a kill voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get(sender);

            if (!Plugin.Instance.Config.EnableKill)
            {
                response = Plugin.Instance.Translation.VoteKillDisabled;
                return false;
            }

            if (!player.CheckPermission("cv.callvotekill"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kill <player> (reason)";
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Plugin.Instance.Config.MaxWaitKill || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitKill - Round.ElapsedTime.TotalSeconds}");
                return false;
            }

            if (args.Count == 1)
            {
                response = Plugin.Instance.Translation.PassReason;
                return false;
            }

            Player locatedPlayer = Player.Get(args.ElementAt(0));

            if (locatedPlayer == null)
            {
                response = Plugin.Instance.Translation.PlayerNotFound.Replace("%Player%", args.ElementAt(0));
                return false;
            }


            List<Player> playerSearch = Player.List.Where(p => p.Nickname.Contains(args.ElementAt(0))).ToList();
            if (playerSearch.Count() < 0 || playerSearch.Count() > 1)
            {
                response = Plugin.Instance.Translation.PlayersWithSameName.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            string reason = args.ElementAt(1);

            options.Add(Plugin.Instance.Translation.CommandYes, Plugin.Instance.Translation.OptionYes);
            options.Add(Plugin.Instance.Translation.CommandNo, Plugin.Instance.Translation.OptionNo);

            VotingAPI.CurrentVoting = new Voting(Plugin.Instance.Translation.AskedToKill
                .Replace("%Player%", player.Nickname)
                .Replace("%Offender%", locatedPlayer.Nickname)
                .Replace("%Reason%", reason),
                options,
                player,
                delegate (Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f); //Just so you know that it exists
                    if (yesVotePercent >= Plugin.Instance.Config.ThresholdKill && yesVotePercent > noVotePercent)
                    {
                        if (!locatedPlayer.CheckPermission("cv.untouchable"))
                        {
                            locatedPlayer.Kill(reason);
                            Map.Broadcast(8, Plugin.Instance.Translation.PlayerKilled
                                .Replace("%VotePercent%", yesVotePercent.ToString())
                                .Replace("%Player%", player.Nickname)
                                .Replace("%Offender%", locatedPlayer.Nickname)
                                .Replace("%Reason%", reason));
                        }
                        if (!locatedPlayer.CheckPermission("cv.untouchable")) locatedPlayer.Kill(reason);
                        if (locatedPlayer.CheckPermission("cv.untouchable")) locatedPlayer.Broadcast(5, Plugin.Instance.Translation.Untouchable.Replace("%VotePercent%", yesVotePercent.ToString()));
                    }
                    else
                    {
                        Map.Broadcast(5, Plugin.Instance.Translation.NotSuccessFullKick
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%ThresholdKick%", Plugin.Instance.Config.ThresholdKick.ToString())
                            .Replace("%Offender%", locatedPlayer.Nickname));
                    }
                });
            response = VotingAPI.CurrentVoting.Response;
            return true;
        }
    }
}