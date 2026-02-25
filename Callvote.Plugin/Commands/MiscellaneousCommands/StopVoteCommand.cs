#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using Callvote.Commands.ParentCommands;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API;
using CommandSystem;

namespace Callvote.Commands.MiscellaneousCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteParentCommand))]
#endif
    public class StopVoteCommand : ICommand
    {
        public string Command => "stopvote";

        public string[] Aliases => ["stop"];

        public string Description => "Stops a vote session.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!VoteHandler.IsVoteActive)
            {
                response = CallvotePlugin.Instance.Translation.NoVoteInProgress;
                return false;
            }
#if EXILED
            if ((player != null && !player.CheckPermission("cv.stopvote")) || (player == null && sender is not ServerConsoleSender))
#else
            if ((player != null && !player.HasPermissions("cv.stopvote")) || (player == null && sender is not ServerConsoleSender))
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (arguments.Count == 0)
            {
                VoteHandler.FinishVote();
                response = CallvotePlugin.Instance.Translation.VoteStoped;
                return false;
            }

            VoteHandler.FinishVote(isForced: true);

            response = CallvotePlugin.Instance.Translation.VoteStoped;
            return true;
        }
    }
}