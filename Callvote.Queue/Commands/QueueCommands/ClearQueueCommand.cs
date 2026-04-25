#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.Queue.Features;
using CommandSystem;
using Mirror;

namespace Callvote.Queue.Commands.QueueCommands
{
/*#if !EXILED
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
#endif*/
    public class ClearQueueCommand : ICommand
    {
        public string Command => "clear";

        public string[] Aliases => ["clq", "clr"];

        public string Description => "Clears the Vote Queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!QueuePlugin.Instance.Config.EnableQueue)
            {
                response = QueuePlugin.Instance.Translation.QueueDisabled;
                return false;
            }

            Player player = Player.Get(sender);

#if EXILED
            if ((player != null && !player.CheckPermission("cv.managequeue")) || (player == null && sender is not ServerConsoleSender))
#else
            if ((player != null && !player.HasPermissions("cv.managequeue")) || (player == null && sender is not ServerConsoleSender))
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            MaxVotesAndQueue.VoteQueue.Clear();
            response = QueuePlugin.Instance.Translation.QueueCleared;
            return true;
        }
    }
}