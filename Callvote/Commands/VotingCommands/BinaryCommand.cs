using Callvote.API;
using Callvote.API.VotingsTemplate;
using Callvote.Enums;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace Callvote.Commands.VotingCommands
{
    public class BinaryCommand : ICommand
    {
        public string Command => "binary";

        public string[] Aliases => new[] { "binario", "bi", "b" };

        public string Description => "Calls a binary voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {

            Player player = Player.Get(sender);

            if (!player.CheckPermission("cv.callvotecustom") || !player.CheckPermission("cv.bypass") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return true;
            }

            VotingHandler.CallVoting(new BinaryVoting(player, Callvote.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname).Replace("%Custom%", string.Join(" ", args)), nameof(VotingTypeEnum.Binary), null));
            response = VotingHandler.Response;
            return true;
        }
    }
}