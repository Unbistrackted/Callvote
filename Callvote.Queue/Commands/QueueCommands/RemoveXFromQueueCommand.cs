using System;
using Callvote.Features;
using Callvote.Features.Extensions;
using Callvote.Queue.Commands.ParentCommands;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;

namespace Callvote.Queue.Commands.QueueCommands
{
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
    public class RemoveXFromQueueCommand : ICommand
    {
        public string Command => "removeindex";

        public string[] Aliases => ["rid", "rd", "ri"];

        public string Description => "Removes X Vote from Vote Queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.EnableQueue)
            {
                response = Plugin.Instance.Translation.QueueDisabled;
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
                response = Plugin.Instance.Translation.InvalidArgument;
                return false;
            }

            int size = MaxVotesAndQueue.VoteQueue.Count;

            MaxVotesAndQueue.VoteQueue.RemoveFromQueue(number);

            response = Plugin.Instance.Translation.RemovedFromQueue.Replace("%Number%", (size - MaxVotesAndQueue.VoteQueue.Count).ToString());
            return true;
        }
    }
}