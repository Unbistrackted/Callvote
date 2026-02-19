#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using Callvote.Commands.ParentCommands;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API;
using Callvote.Features;
using Callvote.Features.Extensions;
using CommandSystem;

namespace Callvote.Commands.QueueCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
#endif
    public class RemoveTypeFromQueueCommand : ICommand
    {
        public string Command => "removetype";

        public string[] Aliases => ["rtype", "rt"];

        public string Description => "Remove a certain Vote type from Vote queue.";

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

            string voteType = arguments.At(0);

            List<Vote> voteToRemove = VoteHandler.VoteQueue.Where(v => v.VoteType.ToLower().Equals(voteType.ToLower())).ToList();

            if (voteToRemove.Count() == 0)
            {
                response = CallvotePlugin.Instance.Translation.TypeNotFound.Replace("%Type%", arguments.At(0));
                return false;
            }

            foreach (Vote vote in voteToRemove)
            {
                VoteHandler.VoteQueue.RemoveItemFromQueue(vote);
            }

            response = CallvotePlugin.Instance.Translation.RemovedFromQueue.Replace("%Number%", voteToRemove.Count.ToString());
            return true;
        }
    }
}