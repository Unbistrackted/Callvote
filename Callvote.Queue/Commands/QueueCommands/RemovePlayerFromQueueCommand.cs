#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.Features.Votes;
using Callvote.Features.Extensions;
using Callvote.Queue.Features;
using CommandSystem;

namespace Callvote.Queue.Commands.QueueCommands
{
/*#if !EXILED
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
#endif*/
    public class RemovePlayerFromQueueCommand : ICommand
    {
        public string Command => "removeplayer";

        public string[] Aliases => ["rplayer", "rp"];

        public string Description => "Remove a certain Player from Vote Queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!QueuePlugin.Instance.Config.EnableQueue)
            {
                response = QueuePlugin.Instance.Translation.QueueDisabled;
                return false;
            }

            Player player = Player.Get(sender);
#if EXILED
            if ((player != null && !player.CheckPermission("cv.managequeue")) || (player == null && sender is not ServerConsoleSender))
#else
            if ((player != null && !player.HasPermissions("cv.managequeue")) || (player == null && sender is not ServerConsoleSender))
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            Player playerToRemove = Player.Get(arguments.At(0));

            if (playerToRemove == null)
            {
                response = CallvotePlugin.Instance.Translation.PlayerNotFound.Replace("%Player%", arguments.At(0));
                return false;
            }

            List<Vote> votesToRemove = [.. MaxVotesAndQueue.VoteQueue.Where(v => v.CallVotePlayer.Equals(playerToRemove.ReferenceHub))];

            if (!votesToRemove.Any())
            {
                response = CallvotePlugin.Instance.Translation.PlayerNotFound.Replace("%Player%", playerToRemove?.Nickname ?? string.Empty);
                return false;
            }

            foreach (Vote vote in votesToRemove)
            {
                MaxVotesAndQueue.VoteQueue.RemoveItemFromQueue(vote);
            }

            response = QueuePlugin.Instance.Translation.RemovedFromQueue.Replace("%Number%", votesToRemove.Count.ToString());
            return true;
        }
    }
}