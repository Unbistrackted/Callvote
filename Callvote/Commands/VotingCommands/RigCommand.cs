#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using Callvote.Commands.ParentCommands;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using System.Linq;
using Callvote.API;
using CommandSystem;

namespace Callvote.Commands.VotingCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteCommand))]
#endif
    public class RigCommand : ICommand
    {
        public string Command => "rig";

        public string[] Aliases => ["r"];

        public string Description => "Rigs the system.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
#if EXILED
            if (!player.CheckPermission("cv.superadmin+") && player != null)
#else
            if (!player.HasPermissions("cv.superadmin+") && player != null)
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (!VotingHandler.IsVotingActive)
            {
                response = CallvotePlugin.Instance.Translation.NoVotingInProgress;
                return false;
            }

            if (arguments.Count < 0)
            {
                response = "You need to pass an option.";
                return false;
            }

            if (arguments.Count == 1)
            {
                response = VotingHandler.CurrentVoting.Rig(arguments.ElementAt(0));
                return true;
            }

            if (!int.TryParse(arguments.ElementAt(1), out int votes))
            {
                response = "Invalid Argument";
                return false;
            }

            response = VotingHandler.CurrentVoting.Rig(arguments.ElementAt(0), amount: votes);
            return true;
        }
    }
}