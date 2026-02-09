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
            if (!player.CheckPermission("cv.callvotecustom") && player != null)
#else
            if (!player.HasPermissions("cv.callvotecustom") && player != null)
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            response = VoteHandler.CallVote(new BinaryVote(player, CallvotePlugin.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname).Replace("%Custom%", string.Join(" ", args)), nameof(VoteTypeEnum.Binary), null));
            return true;
        }
    }
}