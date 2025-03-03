using System;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    internal class RigCommand : ICommand
    {
        public string Command => "rig";

        public string[] Aliases => null;

        public string Description => "";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (!player.CheckPermission("cv.superadmin+"))
            {
                response = "You can't rig the system :trollface:";
                return false;
            }

            if (arguments.Count < 0)
            {
                response = "No arguments passed";
                return false;
            }

            if (!VoteAPI.CurrentVote.Options.ContainsKey(arguments.ElementAt(0))) response = "Couldnt find Key";

            VoteAPI.Rigging(arguments.ElementAt(0));
            response = arguments.ElementAt(0);
            return true;
        }
    }
}