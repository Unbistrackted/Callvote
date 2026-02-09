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

namespace Callvote.Commands.QueueCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
#endif
    public class PauseQueueCommand : ICommand
    {
        public string Command => "pause";

        public string[] Aliases => ["p"];

        public string Description => "Pauses the vote queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!CallvotePlugin.Instance.Config.EnableQueue)
            {
                response = CallvotePlugin.Instance.Translation.QueueDisabled;
                return false;
            }

            Player player = Player.Get(sender);
#if EXILED
            if (!player.CheckPermission("cv.managequeue"))
#else
            if (!player.HasPermissions("cv.managequeue"))
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (!VoteHandler.IsQueuePaused)
            {
                VoteHandler.IsQueuePaused = true;
                response = CallvotePlugin.Instance.Translation.QueuePaused;
                return true;
            }

            VoteHandler.IsQueuePaused = false;
            VoteHandler.DequeueVote();

            response = CallvotePlugin.Instance.Translation.QueueResumed;
            return true;
        }
    }
}