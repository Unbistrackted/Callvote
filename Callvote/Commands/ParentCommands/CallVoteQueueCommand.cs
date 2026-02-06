#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API;
using Callvote.Commands.QueueCommands;
using Callvote.Features;
using CommandSystem;

namespace Callvote.Commands.ParentCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteCommand))]
#endif
    public class CallVoteQueueCommand : ParentCommand
    {
        public CallVoteQueueCommand() => this.LoadGeneratedCommands();

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
