using System;
using Callvote.API.Features.Votes;
using Callvote.Commands.ParentCommands;
using Callvote.Features;
using CommandSystem;

namespace Callvote.Queue.Commands.ParentCommands
{
    [CommandHandler(typeof(CallVoteParentCommand))]
    public class CallVoteQueueParentCommand : ParentCommand
    {
        public CallVoteQueueParentCommand() => this.LoadGeneratedCommands();

        public override string Command => "queue";

        public override string[] Aliases => ["q", "que", "qq", "list"];

        public override string Description => "Commands related to Callvote queue.";

        public override void LoadGeneratedCommands()
        {
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
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
