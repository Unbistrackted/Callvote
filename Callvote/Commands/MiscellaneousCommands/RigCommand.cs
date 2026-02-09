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

namespace Callvote.Commands.MiscellaneousCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteParentCommand))]
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

            if (!VoteHandler.IsVoteActive)
            {
                response = CallvotePlugin.Instance.Translation.NoVoteInProgress;
                return false;
            }

            if (arguments.Count < 0)
            {
                response = CallvotePlugin.Instance.Translation.WrongSyntax;
                return false;
            }

            if (arguments.Count == 1)
            {
                response = VoteHandler.CurrentVote.Rig(arguments.ElementAt(0));
                return true;
            }

            if (!int.TryParse(arguments.ElementAt(1), out int votes))
            {
                response = CallvotePlugin.Instance.Translation.InvalidArgument;
                return false;
            }

            response = VoteHandler.CurrentVote.Rig(arguments.ElementAt(0), amount: votes);
            return true;
        }
    }
}