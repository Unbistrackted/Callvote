#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using Callvote.API;
using Callvote.Extensions;
using Callvote.Features;
using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.Commands.QueueCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteQueueCommand))]
#endif
    public class RemovePlayerFromQueueCommand : ICommand
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
#if EXILED
            if (!player.CheckPermission("cv.managequeue"))
#else
            if (!player.HasPermissions("cv.managequeue"))
#endif
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
                VotingHandler.VotingQueue.RemoveItemFromQueue(vote);

            response = Callvote.Instance.Translation.RemovedFromQueue.Replace("%Number%", votingsToRemove.Count.ToString());
            return true;
        }
    }
}