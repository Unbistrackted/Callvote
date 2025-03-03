using System;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;

namespace Callvote.Commands
{
    public class VoteCommand : ICommand
    {
        public VoteCommand(string command, string[] aliases)
        {
            Command = command;
            Aliases = aliases;
        }

        public string Command { get; set; }

        public string[] Aliases { get; set; }

        public string Description { get; } = "";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (VoteAPI.CurrentVote == null)
            {
                response = Plugin.Instance.Translation.NoCallVoteInProgress;
                return true;
            }

            response = VoteAPI.Voting(player, Command);
            return true;
        }
    }
}