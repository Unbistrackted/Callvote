#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using Callvote.API;
using Callvote.API.VotingsTemplate;
using Callvote.Commands.ParentCommands;
using CommandSystem;
using System;

namespace Callvote.Commands.VotingCommands
{
    [CommandHandler(typeof(CallVoteCommand))]
    public class RestartRoundCommand : ICommand
    {
        public string Command => "restartround";

        public string[] Aliases => ["restart", "rround"];

        public string Description => "Calls a restart round voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableRoundRestart)
            {
                response = Callvote.Instance.Translation.VoteRestartRoundDisabled;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.callvoterestartround") && player != null)
#else
            if (!player.HasPermissions("cv.callvoterestartround") && player != null)
#endif
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitRestartRound)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRestartRound - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitRestartRound)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRestartRound - Round.Duration.TotalSeconds:F0}");
#endif
                return false;
            }

            VotingHandler.CallVoting(new RestartRoundVoting(player));
            response = VotingHandler.Response;
            return true;
        }
    }
}