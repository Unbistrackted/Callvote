#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API;
using Callvote.Features;
using CommandSystem;

namespace Callvote.Commands.VotingCommands
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

            if (!VotingHandler.IsVotingActive)
            {
                response = CallvotePlugin.Instance.Translation.NoVotingInProgress;
                return false;
            }

            if (!VotingHandler.CurrentVoting.TryGetVoteFromCommand(this.Command, out Vote vote))
            {
                response = CallvotePlugin.Instance.Translation.NoOptionAvailable.Replace("%Option%", this.Command);
                return false;
            }

            response = VotingHandler.CurrentVoting.VoteOption(player, vote);
            return true;
        }
    }
}
