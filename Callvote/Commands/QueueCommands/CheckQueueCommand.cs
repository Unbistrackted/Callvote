using Callvote.API;
using Callvote.Features;
using CommandSystem;
using Exiled.API.Features;
using System;

namespace Callvote.Commands.QueueCommands
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

            if (VotingHandler.VotingQueue.Count == 0)
            {
                response = Callvote.Instance.Translation.NoVotingInQueue;
                return false;
            }
            string votingsInfo = string.Empty;
            int _counter = 0;
            foreach (Voting voting in VotingHandler.VotingQueue)
            {
                votingsInfo += $"\nVoting {_counter} ----- Type {voting.VotingType} ----- {voting.Question}\n";
                _counter++;
            }
            response = votingsInfo;
            return true;
        }
    }
}