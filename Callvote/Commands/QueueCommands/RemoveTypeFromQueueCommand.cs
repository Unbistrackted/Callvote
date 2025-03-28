using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    internal class RemoveTypeFromQueueCommand : ICommand
    {
        public string Command => "removetype";

        public string[] Aliases => new[] { "type" };

        public string Description => "Remove a certain Voting type from Voting queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (VotingAPI.CurrentVoting == null)
            {
                response = Callvote.Instance.Translation.NoVotingInProgress;
                return false;
            }
            if (!player.CheckPermission("cv.queue"))
            {
                response = Callvote.Instance.Translation.NoPermissionToVote;
                return false;
            }

            string votingType = arguments.At(0);

            IEnumerable<Voting> votingsToRemove = VotingAPI.VotingQueue.Where(voting => voting.VotingType == votingType);

            if (votingsToRemove.Count() == 0)
            {
                response = $"No votes of type {votingType} found in the queue.";
                return false;
            }

            foreach (Voting vote in votingsToRemove)
            {
                VotingAPI.VotingQueue.RemoveFromQueue(vote);
            }
            response = Callvote.Instance.Translation.QueueCleared;
            return true;
        }
    }
}