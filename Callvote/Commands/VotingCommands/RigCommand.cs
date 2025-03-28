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
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }
            if (CallvoteAPI.CurrentVoting == null)
            {
                response = Callvote.Instance.Translation.NoVotingInProgress;
                return false;

            }
            if (arguments.Count < 0)
            {
                response = "You need to pass an option.";
                return false;
            }

            if (arguments.Count == 1)
            {
                CallvoteAPI.CurrentVoting.Rig(arguments.ElementAt(0));
                response = CallvoteAPI.Response;
                return true;
            }

            if (!int.TryParse(arguments.ElementAt(1), out int votes))
            {
                response = "Invalid Argument"; //Callvote.Instance.Translation.InvalidArgument;
                return false;
            }

            CallvoteAPI.CurrentVoting.Rig(arguments.ElementAt(0), amount: votes);
            response = arguments.ElementAt(0);
            return true;


        }
    }
}