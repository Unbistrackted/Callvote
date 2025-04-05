using CommandSystem.Commands.Shared;
using System;
using Callvote.Commands.VotingCommands;
using CommandSystem;

namespace Callvote.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class CallVoteCommand : ParentCommand
    {
        public override string Command => "callvote";

        public override string[] Aliases => new[] { "cv" };

        public override string Description => "Enables player to call votings!";

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            response = "Wrong Syntax, use .callvote help";
            return false;
        }
    }
}