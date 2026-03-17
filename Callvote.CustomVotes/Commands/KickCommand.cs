using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.Enums;
using Callvote.API.Features.Votes;
using Callvote.Commands.ParentCommands;
using Callvote.CustomVotes.Features.PredefinedVotes;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;

namespace Callvote.CustomVotes.Commands
{
    [CommandHandler(typeof(CallVoteParentCommand))]
    public class KickCommand : ICommand
    {
        public string Command => "kick";

        public string[] Aliases => ["ki", "k"];

        public string Description => "Calls a kick vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Plugin.Instance.Config.EnableKick)
            {
                response = Plugin.Instance.Translation.VoteKickDisabled;
                return false;
            }

            if ((player != null && !player.HasPermissions("cv.callvotekick")) || (player == null && sender is not ServerConsoleSender))
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kick [player] [reason]";
                return false;
            }

            if (player != null && !player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Plugin.Instance.Config.MaxWaitKick)
            {
                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitKick - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            if (args.Count == 1)
            {
                response = Plugin.Instance.Translation.PassReason;
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

            CallVoteStatus status = VoteHandler.CallVote(new KickVote(player ?? Server.Host, locatedPlayer, reason));

            response = VoteHandler.CurrentVote.GetMessageFromCallVoteStatus(status);
            return true;
        }
    }
}