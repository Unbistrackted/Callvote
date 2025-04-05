﻿using Callvote.API;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;

namespace Callvote.Commands.QueueCommands
{
    internal class ClearQueueCommand : ICommand
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

            if (!player.HasPermissions("cv.managequeue"))
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