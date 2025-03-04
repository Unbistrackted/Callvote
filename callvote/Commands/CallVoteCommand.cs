using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ParentCallVoteCommand : ParentCommand
    {
        public ParentCallVoteCommand()
        {
            LoadGeneratedCommands();
        }

        public override string Command => "callvote";

        public override string[] Aliases => new[] { "cv"};

        public override string Description => "Enables player to call votings!";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new KickCommand());
            RegisterCommand(new KillCommand());
            RegisterCommand(new NukeCommand());
            RegisterCommand(new RespawnWaveCommand());
            RegisterCommand(new RestartRoundCommand());
            RegisterCommand(new RigCommand());
            RegisterCommand(new StopVoteCommand());
            RegisterCommand(new BinaryCommand());
            RegisterCommand(new CustomVotingCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            response = "Wrong Syntax, please use .callvote help";
            return false;
        }
    }
}