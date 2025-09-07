using Callvote.API;
using Callvote.API.VotingsTemplate;
using Callvote.Commands.ParentCommands;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;

namespace Callvote.Commands.VotingCommands
{
    [CommandHandler(typeof(CallVoteCommand))]
    public class FFCommand : ICommand
    {
        public string Command => "friendlyfire";

        public string[] Aliases => new[] { "ff" };

        public string Description => "Calls a enable/disable ff voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableFf)
            {
                response = Callvote.Instance.Translation.VoteFFDisabled;
                return false;
            }

            if (!player.HasPermissions("cv.callvoteff") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitFf)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitFf - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            string question;

            if (!Server.FriendlyFire)
            {
                question = Callvote.Instance.Translation.AskedToDisableFf;
            }
            else
            {
                question = Callvote.Instance.Translation.AskedToEnableFf;
            }

            VotingHandler.CallVoting(new FFVoting(player));
            response = VotingHandler.Response;
            return true;
        }
    }
}