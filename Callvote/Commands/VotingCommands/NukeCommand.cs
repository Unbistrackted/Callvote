using Callvote.API;
using Callvote.API.VotingsTemplate;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace Callvote.Commands.VotingCommands
{
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

            if (!player.CheckPermission("cv.callvotenuke") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitNuke)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitNuke - Round.ElapsedTime.TotalSeconds:F0}");
                return false;
            }

            VotingHandler.CallVoting(new NukeVoting(player));
            response = VotingHandler.Response;
            return true;
        }
    }
}