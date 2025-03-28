using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.Commands
{
    internal class RemoveTypeFromQueueCommand : ICommand
    {
        public string Command => "removetype";

        public string[] Aliases => new[] { "rtype", "rt" };

        public string Description => "Remove a certain Voting type from Voting queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Callvote.Instance.Config.EnableQueue)
            {
                response = Callvote.Instance.Translation.QueueDisabled;
                return false;
            }

            Player player = Player.Get(sender);

            if (!player.CheckPermission("cv.managequeue"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            string votingType = arguments.At(0);

            List<Voting> votingsToRemove = CallvoteAPI.VotingQueue.Where(voting => voting.VotingType.ToLower().Equals(votingType.ToLower())).ToList();

            if (votingsToRemove.Count() == 0)
            {
                response = Callvote.Instance.Translation.TypeNotFound.Replace("%Type%", arguments.At(0));
                return false;
            }

            foreach (Voting vote in votingsToRemove)
            {
                CallvoteAPI.VotingQueue.RemoveFromQueuePatch(vote);
            }
            response = Callvote.Instance.Translation.RemovedFromQueue.Replace("%Number%", votingsToRemove.Count.ToString());
            return true;
        }
    }
}