using Callvote.API;
using Callvote.API.VotingsTemplate;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.Commands.VotingCommands
{
    public class KillCommand : ICommand
    {
        public string Command => "kill";

        public string[] Aliases => new[] { "death", "kil" };

        public string Description => "Calls a kill voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableKill)
            {
                response = Callvote.Instance.Translation.VoteKillDisabled;
                return false;
            }

            if (!player.CheckPermission("cv.callvotekill"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kill <player> (reason)";
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitKill || !player.CheckPermission("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitKill - Round.ElapsedTime.TotalSeconds}");
                return false;
            }

            if (args.Count == 1)
            {
                response = Callvote.Instance.Translation.PassReason;
                return false;
            }

            Player locatedPlayer = Player.Get(args.ElementAt(0));

            if (locatedPlayer == null)
            {
                response = Callvote.Instance.Translation.PlayerNotFound.Replace("%Player%", args.ElementAt(0));
                return false;
            }


            List<Player> playerSearch = Player.List.Where(p => p.Nickname.Contains(args.ElementAt(0))).ToList();
            if (playerSearch.Count() < 0 || playerSearch.Count() > 1)
            {
                response = Callvote.Instance.Translation.PlayersWithSameName.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            string reason = args.ElementAt(1);

            VotingHandler.CallVoting(new KillVoting(player, locatedPlayer, reason));
            response = VotingHandler.Response;
            return true;
        }
    }
}