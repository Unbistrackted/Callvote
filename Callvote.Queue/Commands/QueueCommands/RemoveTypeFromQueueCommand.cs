using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.Features.Votes;
using Callvote.Features;
using Callvote.Features.Extensions;
using Callvote.Queue.Commands.ParentCommands;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;

namespace Callvote.Queue.Commands.QueueCommands
{
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
    public class RemoveTypeFromQueueCommand : ICommand
    {
        public string Command => "removetype";

        public string[] Aliases => ["rtype", "rt"];

        public string Description => "Remove a certain Vote type from Vote queue.";

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

            string voteType = arguments.At(0);

            List<Vote> voteToRemove = MaxVotesAndQueue.VoteQueue.Where(v => v.Type.ToLower().Equals(voteType.ToLower())).ToList();

            if (voteToRemove.Count() == 0)
            {
                response = Plugin.Instance.Translation.TypeNotFound.Replace("%Type%", arguments.At(0));
                return false;
            }

            foreach (Vote vote in voteToRemove)
            {
                MaxVotesAndQueue.VoteQueue.RemoveItemFromQueue(vote);
            }

            response = Plugin.Instance.Translation.RemovedFromQueue.Replace("%Number%", voteToRemove.Count.ToString());
            return true;
        }
    }
}