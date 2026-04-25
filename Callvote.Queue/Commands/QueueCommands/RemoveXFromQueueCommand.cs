#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.Features.Extensions;
using Callvote.Queue.Features;
using CommandSystem;

namespace Callvote.Queue.Commands.QueueCommands
{
/*#if !EXILED
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
#endif*/
    public class RemoveXFromQueueCommand : ICommand
    {
        public string Command => "removeindex";

        public string[] Aliases => ["rid", "rd", "ri"];

        public string Description => "Removes X Vote from Vote Queue.";

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

            if (!int.TryParse(arguments.At(0), out int number))
            {
                response = CallvotePlugin.Instance.Translation.InvalidArgument;
                return false;
            }

            int size = MaxVotesAndQueue.VoteQueue.Count;

            MaxVotesAndQueue.VoteQueue.RemoveFromQueue(number);

            response = QueuePlugin.Instance.Translation.RemovedFromQueue.Replace("%Number%", (size - MaxVotesAndQueue.VoteQueue.Count).ToString());
            return true;
        }
    }
}