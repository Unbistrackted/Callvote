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
    public class ClearQueueCommand : ICommand
    {
        public string Command => "clear";

        public string[] Aliases => ["clq"];

        public string Description => "Clears the Vote Queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!CallvotePlugin.Instance.Config.EnableQueue)
            {
                response = CallvotePlugin.Instance.Translation.QueueDisabled;
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

            VoteHandler.VoteQueue.Clear();
            response = CallvotePlugin.Instance.Translation.QueueCleared;
            return true;
        }
    }
}