#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.Features.Votes;
using Callvote.Features.Extensions;
using Callvote.Queue.Features;
using CommandSystem;

namespace Callvote.Queue.Commands.QueueCommands
{
/*#if !EXILED
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
#endif*/
    public class RemoveTypeFromQueueCommand : ICommand
    {
        public string Command => "removetype";

        public string[] Aliases => ["rtype", "rt"];

        public string Description => "Remove a certain Vote type from Vote queue.";

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

            string voteType = arguments.At(0);

            List<Vote> voteToRemove = MaxVotesAndQueue.VoteQueue.Where(v => v.Type.ToLower().Equals(voteType.ToLower())).ToList();

            if (!voteToRemove.Any())
            {
                response = QueuePlugin.Instance.Translation.TypeNotFound.Replace("%Type%", arguments.At(0));
                return false;
            }

            foreach (Vote vote in voteToRemove)
            {
                MaxVotesAndQueue.VoteQueue.RemoveItemFromQueue(vote);
            }

            response = QueuePlugin.Instance.Translation.RemovedFromQueue.Replace("%Number%", voteToRemove.Count.ToString());
            return true;
        }
    }
}