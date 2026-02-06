#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API;
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
                return true;
            }

            response = VotingHandler.CurrentVoting.Vote(player, this.Command);
            return true;
        }
    }
}