#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API;
using Callvote.Features;
using CommandSystem;

namespace Callvote.Commands.MiscellaneousCommands
{
    public class VoteCommand : ICommand
    {
        public VoteCommand(string command)
        {
            this.Command = command;
        }

        public string Command { get; set; }

        public string[] Aliases { get; set; }

        public string Description { get; } = "Work as a custom vote/option command";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!VoteHandler.IsVoteActive)
            {
                response = CallvotePlugin.Instance.Translation.NoVoteInProgress;
                return false;
            }

            if (!VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(Command, out VoteOption vote))
            {
                response = CallvotePlugin.Instance.Translation.NoOptionAvailable.Replace("%Option%", Command);
                return false;
            }

            response = VoteHandler.CurrentVote.SubmitVoteOption(player, vote);
            return true;
        }
    }
}
