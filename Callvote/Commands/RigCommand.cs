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

        public string[] Aliases => new[] { "r" };

        public string Description => "Rigs the system.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!player.CheckPermission("cv.superadmin+"))
            {
                response = Callvote.Instance.Translation.NoPermissionToVote;
                return false;
            }
            if (VotingAPI.CurrentVoting == null)
            {
                response = Callvote.Instance.Translation.NoVotingInProgress;
                return false;

            }
            if (arguments.Count < 0)
            {
                response = "You need to pass an option.";
                return false;
            }

            if (!VotingAPI.CurrentVoting.Options.ContainsKey(arguments.ElementAt(0)))
            {
                response = Callvote.Instance.Translation.NoOptionAvailable.Replace("%Option%", arguments.ElementAt(0));
                return false;
            }

            VotingAPI.Rig(arguments.ElementAt(0));
            response = arguments.ElementAt(0);
            return true;


        }
    }
}