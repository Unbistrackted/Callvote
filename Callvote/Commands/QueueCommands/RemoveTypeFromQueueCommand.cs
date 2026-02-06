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
    [CommandHandler(typeof(CallVoteQueueCommand))]
#endif
    public class RemoveTypeFromQueueCommand : ICommand
    {
        public string Command => "removetype";

        public string[] Aliases => ["rtype", "rt"];

        public string Description => "Remove a certain Voting type from Voting queue.";

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

            string votingType = arguments.At(0);

            List<Voting> votingsToRemove = VotingHandler.VotingQueue.Where(voting => voting.VotingType.ToLower().Equals(votingType.ToLower())).ToList();

            if (votingsToRemove.Count() == 0)
            {
                response = CallvotePlugin.Instance.Translation.TypeNotFound.Replace("%Type%", arguments.At(0));
                return false;
            }

            foreach (Voting vote in votingsToRemove)
            {
                VotingHandler.VotingQueue.RemoveItemFromQueue(vote);
            }

            response = CallvotePlugin.Instance.Translation.RemovedFromQueue.Replace("%Number%", votingsToRemove.Count.ToString());
            return true;
        }
    }
}