#if !EXILED
using Callvote.Queue.Commands.ParentCommands;
#endif
using System;
using Callvote.API.Features.Votes;
using Callvote.Queue.Features;
using CommandSystem;

namespace Callvote.Queue.Commands.QueueCommands
{
/*#if !EXILED
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
#endif*/
    public class CheckQueueCommand : ICommand
    {
        public string Command => "check";

        public string[] Aliases => ["chk"];

        public string Description => "Checks the vote queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!QueuePlugin.Instance.Config.EnableQueue)
            {
                response = QueuePlugin.Instance.Translation.QueueDisabled;
                return false;
            }

            if (MaxVotesAndQueue.VoteQueue.Count == 0)
            {
                response = QueuePlugin.Instance.Translation.NoVoteInQueue;
                return false;
            }

            string votesInfo = string.Empty;
            int counter = 0;

            foreach (Vote vote in MaxVotesAndQueue.VoteQueue)
            {
                votesInfo += $"\nVote {counter} ----- Type {vote.Type} ----- {vote.Question}\n";
                counter++;
            }

            response = votesInfo;
            return true;
        }
    }
}