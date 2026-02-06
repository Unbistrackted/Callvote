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
using Callvote.Features.Extensions;
using CommandSystem;

namespace Callvote.Commands.QueueCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteQueueCommand))]
#endif
    public class RemoveXFromQueueCommand : ICommand
    {
        public string Command => "removeindex";

        public string[] Aliases => ["rid", "rd", "ri"];

        public string Description => "Removes X voting from voting queue.";

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

            if (!int.TryParse(arguments.At(0), out int number))
            {
                response = CallvotePlugin.Instance.Translation.InvalidArgument;
                return false;
            }

            int size = VotingHandler.VotingQueue.Count;

            VotingHandler.VotingQueue.RemoveFromQueue(number);

            response = CallvotePlugin.Instance.Translation.RemovedFromQueue.Replace("%Number%", (size - VotingHandler.VotingQueue.Count).ToString());
            return true;
        }
    }
}