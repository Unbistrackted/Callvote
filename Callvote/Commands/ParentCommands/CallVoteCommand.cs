using CommandSystem;
using System;

namespace Callvote.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ParentCallVoteCommand : ParentCommand
    {
        public override string Command => "callvote";

        public override string[] Aliases => new[] { "cv" };

        public override string Description => "Enables player to call votings!";

        public ParentCallVoteCommand() => LoadGeneratedCommands();

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new ParentCallVoteQueueCommand());
            RegisterCommand(new KickCommand());
            RegisterCommand(new KillCommand());
            RegisterCommand(new NukeCommand());
            RegisterCommand(new RespawnWaveCommand());
            RegisterCommand(new RestartRoundCommand());
            RegisterCommand(new RigCommand());
            RegisterCommand(new StopVoteCommand());
            RegisterCommand(new BinaryCommand());
            RegisterCommand(new CustomVotingCommand());
            RegisterCommand(new HelpCommand());
            RegisterCommand(new FFCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            response = "Wrong Syntax, use .callvote help";
            return false;
        }
    }
}