#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using Callvote.Commands.ParentCommands;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API;
using Callvote.Features.PredefinedVotes;
using CommandSystem;

namespace Callvote.Commands.CallVoteCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteParentCommand))]
#endif
    public class RestartRoundCommand : ICommand
    {
        public string Command => "restartround";

        public string[] Aliases => ["restart", "rround"];

        public string Description => "Calls a restart round vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!CallvotePlugin.Instance.Config.EnableRoundRestart)
            {
                response = CallvotePlugin.Instance.Translation.VoteRestartRoundDisabled;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.callvoterestartround") && player != null)
#else
            if (!player.HasPermissions("cv.callvoterestartround") && player != null)
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitRestartRound)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitRestartRound - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitRestartRound)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitRestartRound - Round.Duration.TotalSeconds:F0}");
#endif
                return false;
            }

            response = VoteHandler.CallVote(new RestartRoundVote(player));
            return true;
        }
    }
}
