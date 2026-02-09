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
    [CommandHandler(typeof(CallVoteQueueParentCommand))]
#endif
    public class CheckQueueCommand : ICommand
    {
        public string Command => "check";

        public string[] Aliases => ["chk"];

        public string Description => "Checks the vote queue.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!CallvotePlugin.Instance.Config.EnableQueue)
            {
                response = CallvotePlugin.Instance.Translation.QueueDisabled;
                return false;
            }

            Player player = Player.Get(sender);

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