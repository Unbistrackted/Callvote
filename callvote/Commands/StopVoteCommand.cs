using System;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    internal class StopVoteCommand : ICommand
    {
        public string Command => "stopvote";

        public string[] Aliases => new[] { "stop" };

        public string Description => "";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            if (!player.CheckPermission("cv.callvoterestartround") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return false;
            }

            response = VoteAPI.StopVote();
            return true;
        }
    }
}