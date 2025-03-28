using System;
using CommandSystem;
using Exiled.API.Features;

namespace Callvote.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ParentCallVoteQueueCommand : ParentCommand
    {
        public ParentCallVoteQueueCommand()
        {
            LoadGeneratedCommands();
        }

        public override string Command => "callvotequeue";

        public override string[] Aliases => new[] { "cvqueue", "cvq", "cv.queue" };

        public override string Description => "Enables player to call votings!";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new CheckQueueCommand());
            RegisterCommand(new ClearQueueCommand());
            RegisterCommand(new RemovePlayerFromQueueCommand());
            RegisterCommand(new RemoveTypeFromQueueCommand());
            RegisterCommand(new RemoveXFromQueueCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            response = "Wrong Syntax, use .callvote help";
            return false;
        }
    }
}