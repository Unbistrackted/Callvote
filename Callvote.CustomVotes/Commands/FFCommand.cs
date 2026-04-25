using System;
using Callvote.API.Enums;
using Callvote.API.Features.Votes;
using Callvote.Commands.ParentCommands;
using Callvote.CustomVotes.Features.PredefinedVotes;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;

namespace Callvote.CustomVotes.Commands
{
    public class FFCommand : ICommand
    {
        public string Command => "friendlyfire";

        public string[] Aliases => ["ff"];

        public string Description => "Calls a enable/disable ff vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!CustomVotePlugin.Instance.Config.EnableFf)
            {
                response = CustomVotePlugin.Instance.Translation.VoteFFDisabled;
                return false;
            }

            if ((player != null && !player.HasPermissions("cv.callvoteff")) || (player == null && sender is not ServerConsoleSender))
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (player != null && !player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CustomVotePlugin.Instance.Config.MaxWaitFf)
            {
                response = CustomVotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CustomVotePlugin.Instance.Config.MaxWaitFf - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            string question = Server.FriendlyFire ? CustomVotePlugin.Instance.Translation.AskedToEnableFf : CustomVotePlugin.Instance.Translation.AskedToDisableFf;

            CallVoteStatus status = VoteHandler.CallVote(new FFVote(player ?? Server.Host));

            response = VoteHandler.CurrentVote.GetMessageFromCallVoteStatus(status);
            return true;
        }
    }
}