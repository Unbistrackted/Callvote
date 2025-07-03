using Callvote.API;
using Callvote.API.VotingsTemplate;
using CommandSystem;
using LabApi.Features.Wrappers;
using LabApi.Features.Permissions;
using System;
using Callvote.Commands.ParentCommands;

namespace Callvote.Commands.VotingCommands
{
    [CommandHandler(typeof(CallVoteCommand))]
    public class NukeCommand : ICommand
    {
        public string Command => "nuke";

        public string[] Aliases => new[] { "nu", "bomba" };

        public string Description => "Calls a nuke voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableNuke)
            {
                response = Callvote.Instance.Translation.VoteNukeDisabled;
                return false;
            }

            if (!player.HasPermissions("cv.callvotenuke") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitNuke)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitNuke - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            VotingHandler.CallVoting(new NukeVoting(player));
            response = VotingHandler.Response;
            return true;
        }
    }
}