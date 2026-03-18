using System;
using Callvote.Features;
using Callvote.Queue.Commands.ParentCommands;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;

namespace Callvote.Queue.Commands.QueueCommands
{
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
    public class PauseQueueCommand : ICommand
    {
        public string Command => "pause";

        public string[] Aliases => ["p"];

        public string Description => "Pauses the vote queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.EnableQueue)
            {
                response = Plugin.Instance.Translation.QueueDisabled;
                return false;
            }

            Player player = Player.Get(sender);

            if ((player != null && !player.HasPermissions("cv.managequeue")) || (player == null && sender is not ServerConsoleSender))
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (!MaxVotesAndQueue.IsQueuePaused)
            {
                MaxVotesAndQueue.IsQueuePaused = true;
                response = Plugin.Instance.Translation.QueuePaused;
                return true;
            }

            MaxVotesAndQueue.IsQueuePaused = false;
            MaxVotesAndQueue.DequeueVote();

            response = Plugin.Instance.Translation.QueueResumed;
            return true;
        }
    }
}