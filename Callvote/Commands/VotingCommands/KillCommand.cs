#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using Callvote.API;
using Callvote.Features.PredefinedVotings;
using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.Commands.VotingCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteCommand))]
#endif
    public class KillCommand : ICommand
    {
        public string Command => "kill";

        public string[] Aliases => ["death", "kil"];

        public string Description => "Calls a kill voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableKill)
            {
                response = Callvote.Instance.Translation.VoteKillDisabled;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.callvotekill") && player != null)
#else
            if (!player.HasPermissions("cv.callvotekill") && player != null)
#endif
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kill [player] [reason]";
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitKill)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitKill - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitKill)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitKill - Round.Duration.TotalSeconds:F0}");
#endif
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


            List<Player> playerSearch = [.. Player.List.Where(p => p.Nickname.Contains(args.ElementAt(0)))];

            if (playerSearch.Count() is < 0 or > 1)
            {
                response = Callvote.Instance.Translation.PlayersWithSameName.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            string reason = string.Join(" ", args.Skip(1));

            VotingHandler.CallVoting(new KillVoting(player, locatedPlayer, reason));
            response = VotingHandler.Response;
            return true;
        }
    }
}