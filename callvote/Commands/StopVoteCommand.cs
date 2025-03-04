using System;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    internal class StopVoteCommand : ICommand
    {
        public string Command => "stopvote";

        public string[] Aliases => new[] { "stop" };

        public string Description => "Stops a voting session.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (VotingAPI.CurrentVoting == null)
            {
                response = Plugin.Instance.Translation.NoVotingInProgress;
                return false;
            }

            if (!player.CheckPermission("cv.stopvote") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return false;
            }

            response = VotingAPI.CurrentVoting.Stop();
            return true;
        }
    }
}