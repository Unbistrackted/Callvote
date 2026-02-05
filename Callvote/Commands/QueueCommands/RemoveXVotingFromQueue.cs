#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using Callvote.Commands.ParentCommands;
#endif
using Callvote.API;
using Callvote.Extensions;
using CommandSystem;
using System;

namespace Callvote.Commands.QueueCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteQueueCommand))]
#endif
    public class RemoveXFromQueueCommand : ICommand
    {
        public string Command => "removeindex";

        public string[] Aliases => new[] { "rid", "rd", "ri" };

        public string Description => "Removes X voting from voting queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Callvote.Instance.Config.EnableQueue)
            {
                response = Callvote.Instance.Translation.QueueDisabled;
                return false;
            }

            Player player = Player.Get(sender);

            if (VotingHandler.CurrentVoting == null)
            {
                response = Callvote.Instance.Translation.NoVotingInProgress;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.managequeue"))
#else
            if (!player.HasPermissions("cv.managequeue"))
#endif
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int number))
            {
                response = Callvote.Instance.Translation.InvalidArgument;
                return false;
            }

            int size = VotingHandler.VotingQueue.Count;

            VotingHandler.VotingQueue.RemoveFromQueue(number);

            response = Callvote.Instance.Translation.RemovedFromQueue.Replace("%Number%", (size - VotingHandler.VotingQueue.Count).ToString());
            return true;
        }
    }
}