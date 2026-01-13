#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.API;
using Callvote.Features;
using CommandSystem;
using System;

namespace Callvote.Commands.ParentCommands
{
    [CommandHandler(typeof(CallVoteCommand))]
    public class CallVoteQueueCommand : ParentCommand
    {
        public override string Command => "queue";

        public override string[] Aliases => new[] { "q", "que", "qq", "list" };

        public override string Description => "Commands related to Callvote queue.";

        public CallVoteQueueCommand() => LoadGeneratedCommands();

        public override void LoadGeneratedCommands()
        {
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
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
