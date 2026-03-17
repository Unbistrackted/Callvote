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
    [CommandHandler(typeof(CallVoteParentCommand))]
    public class NukeCommand : ICommand
    {
        public string Command => "nuke";

        public string[] Aliases => ["nu", "bomba"];

        public string Description => "Calls a nuke vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Plugin.Instance.Config.EnableNuke)
            {
                response = Plugin.Instance.Translation.VoteNukeDisabled;
                return false;
            }

            if ((player != null && !player.HasPermissions("cv.callvotenuke")) || (player == null && sender is not ServerConsoleSender))
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (player != null && !player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Plugin.Instance.Config.MaxWaitNuke)
            {
                response = Plugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Plugin.Instance.Config.MaxWaitNuke - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            CallVoteStatus status = VoteHandler.CallVote(new NukeVote(player ?? Server.Host));

            response = VoteHandler.CurrentVote.GetMessageFromCallVoteStatus(status);
            return true;
        }
    }
}