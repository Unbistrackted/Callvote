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
    public class RemovePlayerFromQueueCommand : ICommand
    {
        public string Command => "removeplayer";

        public string[] Aliases => ["rplayer", "rp"];

        public string Description => "Remove a certain Player from Vote Queue.";

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

            Player playerToRemove = Player.Get(arguments.At(0));

            List<Vote> votesToRemove = MaxVotesAndQueue.VoteQueue.Where(v => v.CallVotePlayer.Equals(playerToRemove)).ToList();

            if (votesToRemove.Count() == 0)
            {
                response = Plugin.Instance.Translation.PlayerNotFound.Replace("%Player%", playerToRemove.Nickname);
                return false;
            }

            foreach (Vote vote in votesToRemove)
            {
                MaxVotesAndQueue.VoteQueue.RemoveItemFromQueue(vote);
            }

            response = Plugin.Instance.Translation.RemovedFromQueue.Replace("%Number%", votesToRemove.Count.ToString());
            return true;
        }
    }
}