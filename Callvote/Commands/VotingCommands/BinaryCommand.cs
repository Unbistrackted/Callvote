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
using Callvote.API.VotingsTemplate;
using Callvote.Features.Enums;
using CommandSystem;

namespace Callvote.Commands.VotingCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteCommand))]
#endif
    public class BinaryCommand : ICommand
    {
        public string Command => "binary";

        public string[] Aliases => ["binario", "bi", "b", "yesno"];

        public string Description => "Calls a binary voting.";

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

            response = VotingHandler.CallVoting(new BinaryVoting(player, CallvotePlugin.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname).Replace("%Custom%", string.Join(" ", args)), nameof(VotingTypeEnum.Binary), null));
            return true;
        }
    }
}