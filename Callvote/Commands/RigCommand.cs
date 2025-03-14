using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

namespace Callvote.Commands
{
    internal class RigCommand : ICommand
    {
        public string Command => "rig";

        public string[] Aliases => new[] { "r" };

        public string Description => "Rigs the system.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

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

            if (!VotingAPI.CurrentVoting.Options.ContainsKey(arguments.ElementAt(0)))   
            {
                response = "Couldn't find option";
                return false;
            }

            VotingAPI.Rig(arguments.ElementAt(0));
            response = arguments.ElementAt(0);
            return true;


        }
    }
}