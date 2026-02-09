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
using Callvote.Features.Enums;
using Callvote.Features.PredefinedVotes;
using CommandSystem;

namespace Callvote.Commands.CallVoteCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteParentCommand))]
#endif
    public class NukeCommand : ICommand
    {
        public string Command => "nuke";

        public string[] Aliases => ["nu", "bomba"];

        public string Description => "Calls a nuke vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!CallvotePlugin.Instance.Config.EnableNuke)
            {
                response = CallvotePlugin.Instance.Translation.VoteNukeDisabled;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.callvotenuke") && player != null)
#else
            if (!player.HasPermissions("cv.callvotenuke") && player != null)
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitNuke)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitNuke - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitNuke)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitNuke - Round.Duration.TotalSeconds:F0}");
#endif
                return false;
            }

            CallVoteStatusEnum status = VoteHandler.CallVote(new NukeVote(player));

            response = VoteHandler.GetMessageFromCallVoteStatus(status);
            return true;
        }
    }
}