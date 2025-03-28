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
    internal class RemovePlayerFromQueueCommand : ICommand
    {
        public string Command => "removeplayer";

        public string[] Aliases => new[] { "player" };

        public string Description => "Remove a certain Player from Voting queue.";

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

            Player playerToRemove = Player.Get(arguments.At(0));

            IEnumerable<Voting> votingsToRemove = VotingAPI.VotingQueue.Where(voting => voting.CallVotePlayer == playerToRemove);

            if (votingsToRemove.Count() == 0)
            {
                response = Callvote.Instance.Translation.PlayerNotFound.Replace("%Player%", playerToRemove.Nickname);
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