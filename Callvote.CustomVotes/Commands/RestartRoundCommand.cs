using System;
using Callvote.API.Enums;
using Callvote.API.Features.Votes;
using Callvote.CustomVotes.Features.PredefinedVotes;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;

namespace Callvote.CustomVotes.Commands
{
    public class RestartRoundCommand : ICommand
    {
        public string Command => "restartround";

        public string[] Aliases => ["restart", "rround"];

        public string Description => "Calls a restart round vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!CustomVotePlugin.Instance.Config.EnableRoundRestart)
            {
                response = CustomVotePlugin.Instance.Translation.VoteRestartRoundDisabled;
                return false;
            }

            if ((player != null && !player.HasPermissions("cv.callvoterestartround")) || (player == null && sender is not ServerConsoleSender))
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (player != null && !player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CustomVotePlugin.Instance.Config.MaxWaitRestartRound)
            {
                response = CustomVotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CustomVotePlugin.Instance.Config.MaxWaitRestartRound - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            CallVoteStatus status = VoteHandler.CallVote(new RestartRoundVote(player ?? Server.Host));

            response = VoteHandler.CurrentVote.GetMessageFromCallVoteStatus(status);
            return true;
        }
    }
}
