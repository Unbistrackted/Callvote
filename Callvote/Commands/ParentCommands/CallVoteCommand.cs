using Callvote.Commands.VotingCommands;
using CommandSystem;
using System;

namespace Callvote.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class CallVoteCommand : ParentCommand
    {
        public override string Command => "callvote";

        public override string[] Aliases => new[] { "cv" };

        public override string Description => "Enables player to call votes, manage the queue and more!";

        public CallVoteCommand() => LoadGeneratedCommands();

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new CallVoteQueueCommand());
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
            RegisterCommand(new TranslationCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            response = Callvote.Instance.Translation.WrongSyntax;
            return false;
        }
    }
}