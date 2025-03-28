using System;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;

namespace Callvote.Commands
{
    internal class CheckQueueCommand : ICommand
    {
        public string Command => "check";

        public string[] Aliases => new[] { "chk" };

        public string Description => "Checks the voting queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (VotingAPI.CurrentVoting == null)
            {
                response = Callvote.Instance.Translation.NoVotingInProgress;
                return false;
            }
            string votingsInfo = string.Empty;
            int _counter = 1;
            foreach (Voting voting in VotingAPI.VotingQueue)
            {
                votingsInfo += $"Voting {_counter}: {voting.Question}\n";
                _counter++;
            }
            response = votingsInfo;
            return true;
        }
    }
}