using Callvote.Commands.ParentCommands;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;
using Callvote.API.Enums;
using Callvote.API.Features.Votes;
using CommandSystem;
using Callvote.CustomVotes.Features.PredefinedVotes;

namespace Callvote.CustomVotes.Commands
{
    [CommandHandler(typeof(CallVoteParentCommand))]
    public class FFCommand : ICommand
    {
        public string Command => "friendlyfire";

        public string[] Aliases => ["ff"];

        public string Description => "Calls a enable/disable ff vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!CallvotePlugin.Instance.Config.EnableFf)
            {
                response = CallvotePlugin.Instance.Translation.VoteFFDisabled;
                return false;
            }
            if ((player != null && !player.HasPermissions("cv.callvoteff")) || (player == null && sender is not ServerConsoleSender))
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (player != null && !player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitFf)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitFf - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            string question;

            if (!Server.FriendlyFire)
            {
                question = CallvotePlugin.Instance.Translation.AskedToDisableFf;
            }
            else
            {
                question = CallvotePlugin.Instance.Translation.AskedToEnableFf;
            }

            CallVoteStatus status = VoteHandler.CallVote(new FFVote(player ?? Server.Host));

            response = VoteHandler.CurrentVote.GetMessageFromCallVoteStatus(status);
            return true;
        }
    }
}