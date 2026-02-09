#if EXILED
using Callvote.Commands.CallVoteCommands;
#endif
using System;
using Callvote.Commands.MiscellaneousCommands;
using CommandSystem;

namespace Callvote.Commands.ParentCommands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class CallVoteParentCommand : ParentCommand
    {
        public CallVoteParentCommand() => this.LoadGeneratedCommands();

        public override string Command => "callvote";

        public override string[] Aliases => ["cv"];

        public override string Description => "Enables player to call votes, manage the queue and more!";

        public override void LoadGeneratedCommands()
        {
#if EXILED
            this.RegisterCommand(new CallVoteQueueParentCommand());
            this.RegisterCommand(new KickCommand());
            this.RegisterCommand(new KillCommand());
            this.RegisterCommand(new NukeCommand());
            this.RegisterCommand(new RespawnWaveCommand());
            this.RegisterCommand(new RestartRoundCommand());
            this.RegisterCommand(new RigCommand());
            this.RegisterCommand(new StopVoteCommand());
            this.RegisterCommand(new BinaryCommand());
            this.RegisterCommand(new CustomCommand());
            this.RegisterCommand(new HelpCommand());
            this.RegisterCommand(new FFCommand());
            this.RegisterCommand(new TranslationCommand());
#endif
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            response = CallvotePlugin.Instance.Translation.WrongSyntax;
            return false;
        }
    }
}