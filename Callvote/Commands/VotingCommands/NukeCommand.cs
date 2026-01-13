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
#if !EXILED
    [CommandHandler(typeof(CallVoteCommand))]
#endif
    public class NukeCommand : ICommand
    {
        public string Command => "nuke";

        public string[] Aliases => ["nu", "bomba"];

        public string Description => "Calls a nuke voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableNuke)
            {
                response = Callvote.Instance.Translation.VoteNukeDisabled;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.callvotenuke") && player != null)
#else
            if (!player.HasPermissions("cv.callvotenuke") && player != null)
#endif
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitNuke)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitNuke - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitNuke)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitNuke - Round.Duration.TotalSeconds:F0}");
#endif
                return false;
            }

            VotingHandler.CallVoting(new NukeVoting(player));
            response = VotingHandler.Response;
            return true;
        }
    }
}