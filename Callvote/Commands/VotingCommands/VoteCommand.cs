using Callvote.API;
using CommandSystem;
using LabApi.Features.Wrappers;
using System;

namespace Callvote.Commands.VotingCommands
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

            if (VotingHandler.CurrentVoting == null)
            {
                response = Callvote.Instance.Translation.NoVotingInProgress;
                return true;
            }
            response = VotingHandler.CurrentVoting.Vote(player, Command);
            return true;
        }
    }
}