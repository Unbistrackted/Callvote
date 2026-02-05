#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using Callvote.Commands.ParentCommands;
#endif
using Callvote.API;
using Callvote.API.VotingsTemplate;
using Callvote.Features.Enums;
using CommandSystem;
using System;


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

            if (
#if EXILED
                !player.CheckPermission("cv.callvotecustom")
#else
                !player.HasPermissions("cv.callvotecustom") 
#endif
                && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            VotingHandler.CallVoting(new BinaryVoting(player, Callvote.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname).Replace("%Custom%", string.Join(" ", args)), nameof(VotingTypeEnum.Binary), null));
            response = VotingHandler.Response;
            return true;
        }
    }
}