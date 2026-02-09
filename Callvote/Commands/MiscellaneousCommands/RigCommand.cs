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
using Callvote.Features;
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

            if (arguments.Count == 0)
            {
                response = CallvotePlugin.Instance.Translation.WrongSyntax;
                return false;
            }

            if (arguments.Count == 1)
            {
                if (!VoteHandler.CurrentVote.Rig(arguments.ElementAt(0), out VoteOption v))
                {
                    response = CallvotePlugin.Instance.Translation.NoOptionAvailable.Replace("%Option%", arguments.ElementAt(0));
                    return false;
                }

                response = $"Rigged 1 vote for {v.Detail}!";
                return true;
            }

            if (!int.TryParse(arguments.ElementAt(1), out int votes))
            {
                response = CallvotePlugin.Instance.Translation.InvalidArgument;
                return false;
            }

            if (!VoteHandler.CurrentVote.Rig(arguments.ElementAt(0), out VoteOption vote, votes))
            {
                response = CallvotePlugin.Instance.Translation.NoOptionAvailable.Replace("%Option%", arguments.ElementAt(0));
                return false;
            }

            response = $"Rigged {votes} votes for {vote.Detail}!";
            return true;
        }
    }
}