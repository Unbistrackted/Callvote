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

            if (!Plugin.Instance.Config.EnableFf)
            {
                response = Plugin.Instance.Translation.VoteFFDisabled;
                return false;
            }

            if ((player != null && !player.HasPermissions("cv.callvoteff")) || (player == null && sender is not ServerConsoleSender))
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (player != null && !player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Plugin.Instance.Config.MaxWaitFf)
            {
                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitFf - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            string question = Server.FriendlyFire ? Plugin.Instance.Translation.AskedToEnableFf : Plugin.Instance.Translation.AskedToDisableFf;

            CallVoteStatus status = VoteHandler.CallVote(new FFVote(player ?? Server.Host));

            response = VoteHandler.CurrentVote.GetMessageFromCallVoteStatus(status);
            return true;
        }
    }
}