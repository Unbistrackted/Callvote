using Callvote.API;
using Callvote.API.Objects;
using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.Commands.QueueCommands
{
    internal class RemovePlayerFromQueueCommand : ICommand
    {
        public string Command => "removeplayer";

        public string[] Aliases => new[] { "rplayer", "rp" };

        public string Description => "Remove a certain Player from Voting queue.";

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

            Player playerToRemove = Player.Get(arguments.At(0));

            List<Voting> votingsToRemove = VotingHandler.VotingQueue.Where(voting => voting.CallVotePlayer.Equals(playerToRemove)).ToList();

            if (votingsToRemove.Count() == 0)
            {
                response = Callvote.Instance.Translation.PlayerNotFound.Replace("%Player%", playerToRemove.Nickname);
                return false;
            }

            foreach (Voting vote in votingsToRemove)
            {
                VotingHandler.VotingQueue.RemoveFromQueuePatch(vote);
            }
            response = Callvote.Instance.Translation.RemovedFromQueue.Replace("%Number%", votingsToRemove.Count.ToString());
            return true;
        }
    }
}