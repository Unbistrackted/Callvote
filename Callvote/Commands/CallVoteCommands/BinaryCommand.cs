#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using Callvote.Commands.ParentCommands;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API;
using Callvote.API.VoteTemplate;
using Callvote.Features.Enums;
using CommandSystem;
using LabApi.Features.Console;

namespace Callvote.Commands.CallVoteCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteParentCommand))]
#endif
    public class BinaryCommand : ICommand
    {
        public string Command => "binary";

        public string[] Aliases => ["binario", "bi", "b", "yesno"];

        public string Description => "Calls a binary vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

#if EXILED
            if ((player != null && !player.CheckPermission("cv.callvotecustom")) || (player == null && sender is not ServerConsoleSender))
#else
            if ((player != null && !player.HasPermissions("cv.callvotecustom")) || (player == null && sender is not ServerConsoleSender))
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            CallVoteStatus status = VoteHandler.CallVote(new BinaryVote(player ?? Server.Host, CallvotePlugin.Instance.Translation.AskedCustom.Replace("%Player%", player?.Nickname ?? Server.Host.Nickname).Replace("%Custom%", string.Join(" ", args)), nameof(VoteType.Binary), null));

            response = VoteHandler.GetMessageFromCallVoteStatus(status);
            return true;
        }
    }
}