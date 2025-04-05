using Callvote.API;
using CommandSystem;
using LabApi.Features.Wrappers;
using LabApi.Features.Permissions;
using System;

namespace Callvote.Commands.VotingCommands
{
    internal class StopVoteCommand : ICommand
    {
        public string Command => "stopvote";

        public string[] Aliases => new[] { "stop" };

        public string Description => "Stops a voting session.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (VotingHandler.CurrentVoting == null)
            {
                response = Callvote.Instance.Translation.NoVotingInProgress;
                return false;
            }

            if (!player.HasPermissions("cv.stopvote") || !player.HasPermissions("cv.bypass"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            VotingHandler.FinishVoting();
            response = Callvote.Instance.Translation.VotingStoped;
            return true;
        }
    }
}