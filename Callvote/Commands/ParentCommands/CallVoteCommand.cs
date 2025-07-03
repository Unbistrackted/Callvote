using CommandSystem;
using System;

namespace Callvote.Commands.ParentCommands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class CallVoteCommand : ParentCommand
    {
        public override string Command => "callvote";

        public override string[] Aliases => ["cv"];

        public override string Description => "Enables player to call votes, manage the queue and more!";

        public CallVoteCommand() => LoadGeneratedCommands();

        public override void LoadGeneratedCommands()
        {
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            response = Callvote.Instance.Translation.WrongSyntax;
            return false;
        }
    }
}