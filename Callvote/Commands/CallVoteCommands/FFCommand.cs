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
#if EXILED
            if (!player.CheckPermission("cv.callvoteff") && player != null)
#else
            if (!player.HasPermissions("cv.callvoteff") && player != null)
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitFf)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitFf - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitFf)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitFf - Round.Duration.TotalSeconds:F0}");
#endif
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

            response = VoteHandler.CallVote(new FFVote(player));
            return true;
        }
    }
}