#if EXILED
using Exiled.API.Features;
using Callvote.Commands.QueueCommands;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API;
using Callvote.Features;
using CommandSystem;

namespace Callvote.Commands.ParentCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteParentCommand))]
#endif
    public class CallVoteQueueParentCommand : ParentCommand
    {
        public CallVoteQueueParentCommand() => this.LoadGeneratedCommands();

        public override string Command => "queue";

        public override string[] Aliases => ["q", "que", "qq", "list"];

        public override string Description => "Commands related to Callvote queue.";

        public override void LoadGeneratedCommands()
        {
#if EXILED
            RegisterCommand(new CheckQueueCommand());
            RegisterCommand(new ClearQueueCommand());
            RegisterCommand(new RemovePlayerFromQueueCommand());
            RegisterCommand(new RemoveTypeFromQueueCommand());
            RegisterCommand(new RemoveXFromQueueCommand());
            RegisterCommand(new PauseQueueCommand());
#endif
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!CallvotePlugin.Instance.Config.EnableQueue)
            {
                response = CallvotePlugin.Instance.Translation.QueueDisabled;
                return false;
            }

            if (VoteHandler.VoteQueue.Count == 0)
            {
                response = CallvotePlugin.Instance.Translation.NoVoteInQueue;
                return false;
            }

            string votesInfo = string.Empty;
            int counter = 0;
            foreach (Vote vote in VoteHandler.VoteQueue)
            {
                votesInfo += $"\nVote {counter} ----- Type {vote.VoteType} ----- {vote.Question}\n";
                counter++;
            }

            response = votesInfo;
            return true;
        }
    }
}
