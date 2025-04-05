using Callvote.API;
using CommandSystem;
using LabApi.Features.Wrappers;
using System;
using LabApi.Features.Permissions;

namespace Callvote.Commands.QueueCommands
{
    internal class PauseQueueCommand : ICommand
    {
        public string Command => "pause";

        public string[] Aliases => new[] { "p" };

        public string Description => "Pauses the voting queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Callvote.Instance.Config.EnableQueue)
            {
                response = Callvote.Instance.Translation.QueueDisabled;
                return false;
            }

            Player player = Player.Get(sender);

            if (!player.HasPermissions("cv.managequeue"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (!VotingHandler.IsQueuePaused)
            {
                VotingHandler.IsQueuePaused = true;
                response = Callvote.Instance.Translation.QueuePaused;
                return true;
            }

            VotingHandler.IsQueuePaused = false;
            VotingHandler.TryStartNextVoting();

            response = Callvote.Instance.Translation.QueueResumed;
            return true;
        }
    }
}