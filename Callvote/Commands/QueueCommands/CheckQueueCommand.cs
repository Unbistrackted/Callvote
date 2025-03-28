using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using System;

namespace Callvote.Commands
{
    internal class CheckQueueCommand : ICommand
    {
        public string Command => "check";

        public string[] Aliases => new[] { "chk" };

        public string Description => "Checks the voting queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Callvote.Instance.Config.EnableQueue)
            {
                response = Callvote.Instance.Translation.QueueDisabled;
                return false;
            }

            Player player = Player.Get(sender);

            if (CallvoteAPI.VotingQueue.Count == 0)
            {
                response = Callvote.Instance.Translation.NoVotingInQueue;
                return false;
            }
            string votingsInfo = string.Empty;
            int _counter = 0;
            foreach (Voting voting in CallvoteAPI.VotingQueue)
            {
                votingsInfo += $"\nVoting {_counter} ----- Type {voting.VotingType} ----- {voting.Question}\n";
                _counter++;
            }
            response = votingsInfo;
            return true;
        }
    }
}