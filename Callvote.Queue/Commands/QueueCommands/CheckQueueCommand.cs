using System;
using Callvote.API.Features.Votes;
using Callvote.Features;
using Callvote.Queue.Commands.ParentCommands;
using CommandSystem;

namespace Callvote.Queue.Commands.QueueCommands
{
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
    public class CheckQueueCommand : ICommand
    {
        public string Command => "check";

        public string[] Aliases => ["chk"];

        public string Description => "Checks the vote queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.EnableQueue)
            {
                response = Plugin.Instance.Translation.QueueDisabled;
                return false;
            }

            if (MaxVotesAndQueue.VoteQueue.Count == 0)
            {
                response = Plugin.Instance.Translation.NoVoteInQueue;
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