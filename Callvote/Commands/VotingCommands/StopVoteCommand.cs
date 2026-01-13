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

namespace Callvote.Commands.VotingCommands
{
    [CommandHandler(typeof(CallVoteCommand))]
    public class StopVoteCommand : ICommand
    {
        public string Command => "stopvote";

        public string[] Aliases => ["stop"];

        public string Description => "Stops a voting session.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (VotingHandler.CurrentVoting == null)
            {
                response = Callvote.Instance.Translation.NoVotingInProgress;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.stopvote") && player != null)
#else
            if (!player.HasPermissions("cv.stopvote") && player != null)
#endif
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