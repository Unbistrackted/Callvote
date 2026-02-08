#if EXILED
using Callvote.Commands.VotingCommands;
#endif
using System;
using CommandSystem;

namespace Callvote.Commands.ParentCommands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class CallVoteCommand : ParentCommand
    {
        public CallVoteCommand() => this.LoadGeneratedCommands();

        public override string Command => "callvote";

        public override string[] Aliases => ["cv"];

        public override string Description => "Enables player to call votes, manage the queue and more!";

        public override void LoadGeneratedCommands()
        {
#if EXILED
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
#endif
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            response = CallvotePlugin.Instance.Translation.WrongSyntax;
            return false;
        }
    }
}