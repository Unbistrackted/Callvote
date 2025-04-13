using Callvote.API;
using Callvote.API.VotingsTemplate;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace Callvote.Commands.VotingCommands
{
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

            if (!player.CheckPermission("cv.callvoteff") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitFf || !player.CheckPermission("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRestartRound - Round.ElapsedTime.TotalSeconds}");
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