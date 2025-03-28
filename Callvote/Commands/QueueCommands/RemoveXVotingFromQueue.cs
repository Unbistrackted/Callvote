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
    internal class RemoveXFromQueueCommand : ICommand
    {
        public string Command => "removenumber";

        public string[] Aliases => new[] { "number" };

        public string Description => "Removes X voting from voting queue.";

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

            if (!int.TryParse(arguments.At(0), out int number))
            {
                response = Callvote.Instance.Translation.InvalidArgument;
                return false;
            }

            VotingAPI.VotingQueue.RemoveFromQueue(number);

            response = Callvote.Instance.Translation.QueueCleared;
            return true;
        }
    }
}