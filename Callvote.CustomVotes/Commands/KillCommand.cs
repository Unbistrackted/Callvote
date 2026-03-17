using Callvote.Commands.ParentCommands;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.Enums;
using Callvote.API.Features.Votes;
using CommandSystem;
using Callvote.CustomVotes.Features.PredefinedVotes;

namespace Callvote.CustomVotes.Commands
{
    [CommandHandler(typeof(CallVoteParentCommand))]
    public class KillCommand : ICommand
    {
        public string Command => "kill";

        public string[] Aliases => ["death", "kil"];

        public string Description => "Calls a kill vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!CallvotePlugin.Instance.Config.EnableKill)
            {
                response = CallvotePlugin.Instance.Translation.VoteKillDisabled;
                return false;
            }

            if ((player != null && !player.HasPermissions("cv.callvotekill")) || (player == null && sender is not ServerConsoleSender))
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kill [player] [reason]";
                return false;
            }

            if (player != null && !player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitKill)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitKill - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            if (args.Count == 1)
            {
                response = CallvotePlugin.Instance.Translation.PassReason;
                return false;
            }

            Player locatedPlayer = Player.GetByNickname(args.ElementAt(0));

            if (locatedPlayer == null)
            {
                response = CallvotePlugin.Instance.Translation.PlayerNotFound.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            if (!locatedPlayer.HasPermissions("cv.untouchable"))
            {
                response = "Player is Untouchable! :trollface:";
                return false;
            }

            List<Player> playerSearch = [.. Player.List.Where(p => p.Nickname.Contains(args.ElementAt(0)))];

            if (playerSearch.Count() is < 0 or > 1)
            {
                response = CallvotePlugin.Instance.Translation.PlayersWithSameName.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            string reason = string.Join(" ", args.Skip(1));

            CallVoteStatus status = VoteHandler.CallVote(new KillVote(player ?? Server.Host, locatedPlayer, reason));

            response = VoteHandler.CurrentVote.GetMessageFromCallVoteStatus(status);
            return true;
        }
    }
}