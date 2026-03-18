using System;
using Callvote.Features;
using Callvote.Queue.Commands.ParentCommands;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using Mirror;

namespace Callvote.Queue.Commands.QueueCommands
{
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
    public class ClearQueueCommand : ICommand
    {
        public string Command => "clear";

        public string[] Aliases => ["clq"];

        public string Description => "Clears the Vote Queue.";

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

            MaxVotesAndQueue.VoteQueue.Clear();
            response = Plugin.Instance.Translation.QueueCleared;
            return true;
        }
    }
}