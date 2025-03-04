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

        public string Description => "Calls a kick voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get(sender);

            if (!Plugin.Instance.Config.EnableKick)
            {
                response = Plugin.Instance.Translation.VoteKickDisabled;
                return false;
            }

            if (!player.CheckPermission("cv.callvotekick"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kick <player> (reason)";
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Plugin.Instance.Config.MaxWaitKick || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitKick - Round.ElapsedTime.TotalSeconds}");
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

            VotingAPI.CurrentVoting = new Voting(Plugin.Instance.Translation.AskedToKick
                .Replace("%Player%", player.Nickname)
                .Replace("%Offender%", locatedPlayer.Nickname)
                .Replace("%Reason%", reason),
                options,
                player,
                delegate (Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Plugin.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f); //Just so you know that it exists
                    if (yesVotePercent >= Plugin.Instance.Config.ThresholdKick && yesVotePercent > noVotePercent)
                    {
                        if (!locatedPlayer.CheckPermission("cv.untouchable"))
                        {
                            locatedPlayer.Kick(reason);
                            Map.Broadcast(8, Plugin.Instance.Translation.PlayerKicked
                                .Replace("%VotePercent%", yesVotePercent.ToString())
                                .Replace("%Player%", player.Nickname)
                                .Replace("%Offender%", locatedPlayer.Nickname)
                                .Replace("%Reason%", reason));
                        }
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