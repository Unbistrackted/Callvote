﻿using Callvote.API;
using Callvote.API.VotingsTemplate;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace Callvote.Commands.VotingCommands
{
    public class RestartRoundCommand : ICommand
    {
        public string Command => "restartround";

        public string[] Aliases => new[] { "restart", "rround" };

        public string Description => "Calls a restart round voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableRoundRestart)
            {
                response = Callvote.Instance.Translation.VoteRestartRoundDisabled;
                return false;
            }

            if (!player.CheckPermission("cv.callvoterestartround") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitRestartRound)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRestartRound - Round.ElapsedTime.TotalSeconds:F0}");
                return false;
            }

            VotingHandler.CallVoting(new RestartRoundVoting(player));
            response = VotingHandler.Response;
            return true;
        }
    }
}