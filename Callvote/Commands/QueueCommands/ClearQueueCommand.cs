using System;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    internal class ClearQueueCommand : ICommand
    {
        public string Command => "clear";

        public string[] Aliases => new[] { "clq" };

        public string Description => "Clears the voting queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (VotingAPI.CurrentVoting == null)
            {
                response = Callvote.Instance.Translation.NoVotingInProgress;
                return false;
            }
            if (!player.CheckPermission("cv.clearqueue"))
            {
                response = Callvote.Instance.Translation.NoPermissionToVote;
                return false;
            }

            VotingAPI.VotingQueue.Clear();
            response = Callvote.Instance.Translation.QueueCleared;
            return true;
        }
    }
}