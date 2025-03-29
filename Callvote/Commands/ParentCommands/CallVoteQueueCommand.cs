using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using System;

namespace Callvote.Commands
{
    internal class ParentCallVoteQueueCommand : ParentCommand
    {

        public override string Command => "queue";

        public override string[] Aliases => new[] { "q", "que", "qq", "list" };

        public override string Description => "Commands related to Callvote queue.";

        public ParentCallVoteQueueCommand() => LoadGeneratedCommands();

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new CheckQueueCommand());
            RegisterCommand(new ClearQueueCommand());
            RegisterCommand(new RemovePlayerFromQueueCommand());
            RegisterCommand(new RemoveTypeFromQueueCommand());
            RegisterCommand(new RemoveXFromQueueCommand());
            RegisterCommand(new PauseQueueCommand());
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
