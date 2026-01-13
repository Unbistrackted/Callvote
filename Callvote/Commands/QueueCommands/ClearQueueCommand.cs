#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using Callvote.API;
using Callvote.Commands.ParentCommands;
using CommandSystem;
using System;

namespace Callvote.Commands.QueueCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteQueueCommand))]
#endif
    public class ClearQueueCommand : ICommand
    {
        public string Command => "clear";

        public string[] Aliases => new[] { "clq" };

        public string Description => "Clears the voting queue.";

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

            VotingHandler.VotingQueue.Clear();
            response = Callvote.Instance.Translation.QueueCleared;
            return true;
        }
    }
}