using Callvote.API;
using Callvote.API.VotingsTemplate;
using Callvote.Commands.ParentCommands;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.Commands.VotingCommands
{
    [CommandHandler(typeof(CallVoteCommand))]
    public class KickCommand : ICommand
    {
        public string Command => "kick";

        public string[] Aliases => new[] { "ki", "k" };

        public string Description => "Calls a kick voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {

            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableKick)
            {
                response = Callvote.Instance.Translation.VoteKickDisabled;
                return false;
            }

            if (!player.HasPermissions("cv.callvotekick") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kick <player> (reason)";
                return false;
            }

            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitKick)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitKick - Round.Duration.TotalSeconds:F0}");
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
            if (playerSearch.Count() is < 0 or > 1)
            {
                response = Callvote.Instance.Translation.PlayersWithSameName.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            string reason = args.ElementAt(1);

            VotingHandler.CallVoting(new KickVoting(player, locatedPlayer, reason));
            response = VotingHandler.Response;
            return true;
        }
    }
}