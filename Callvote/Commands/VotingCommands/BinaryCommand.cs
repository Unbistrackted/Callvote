using Callvote.API;
using Callvote.API.VotingsTemplate;
using Callvote.Commands.ParentCommands;
using Callvote.Enums;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;

namespace Callvote.Commands.VotingCommands
{
    [CommandHandler(typeof(CallVoteCommand))]
    public class BinaryCommand : ICommand
    {
        public string Command => "binary";

        public string[] Aliases => new[] { "binario", "bi", "b" };

        public string Description => "Calls a binary voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {

            Player player = Player.Get(sender);

            if (!player.HasPermissions("cv.callvotecustom") && player != null)
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