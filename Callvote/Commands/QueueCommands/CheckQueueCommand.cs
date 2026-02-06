#if EXILED
using Exiled.API.Features;
#else
using Callvote.Commands.ParentCommands;
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API;
using Callvote.Features;
using CommandSystem;

namespace Callvote.Commands.QueueCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteQueueCommand))]
#endif
    public class CheckQueueCommand : ICommand
    {
        public string Command => "check";

        public string[] Aliases => ["chk"];

        public string Description => "Checks the voting queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!CallvotePlugin.Instance.Config.EnableQueue)
            {
                response = CallvotePlugin.Instance.Translation.QueueDisabled;
                return false;
            }

            Player player = Player.Get(sender);

            if (VotingHandler.VotingQueue.Count == 0)
            {
                response = CallvotePlugin.Instance.Translation.NoVotingInQueue;
                return false;
            }

            string votingsInfo = string.Empty;
            int counter = 0;

            foreach (Voting voting in VotingHandler.VotingQueue)
            {
                votingsInfo += $"\nVoting {counter} ----- Type {voting.VotingType} ----- {voting.Question}\n";
                counter++;
            }

            response = votingsInfo;
            return true;
        }
    }
}