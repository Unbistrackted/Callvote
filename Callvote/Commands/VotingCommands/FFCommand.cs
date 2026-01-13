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
    public class FFCommand : ICommand
    {
        public string Command => "friendlyfire";

        public string[] Aliases => ["ff"];

        public string Description => "Calls a enable/disable ff voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableFf)
            {
                response = Callvote.Instance.Translation.VoteFFDisabled;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.callvoteff") && player != null)
#else
            if (!player.HasPermissions("cv.callvoteff") && player != null)
#endif
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitFf)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitFf - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitFf)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitFf - Round.Duration.TotalSeconds:F0}");
#endif
                return false;
            }

            string question;

            if (!Server.FriendlyFire)
                question = Callvote.Instance.Translation.AskedToDisableFf;

            else
                question = Callvote.Instance.Translation.AskedToEnableFf;

            VotingHandler.CallVoting(new FFVoting(player));
            response = VotingHandler.Response;
            return true;
        }
    }
}