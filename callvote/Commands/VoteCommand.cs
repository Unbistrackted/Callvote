using System;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;

namespace Callvote.Commands
{
    public class VoteCommand : ICommand
    {
        public VoteCommand(string command)
        {
            Command = command;
        }

        public string Command { get; set; }

        public string[] Aliases { get; set; }

        public string Description { get; } = "Work as a custom vote/option command";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (VoteAPI.CurrentVoting == null)
            {
                response = Plugin.Instance.Translation.NoVotingInProgress;
                return true;
            }

            response = VoteAPI.Voting(player, Command);
            return true;
        }
    }
}