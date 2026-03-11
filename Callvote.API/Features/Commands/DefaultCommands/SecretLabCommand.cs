#if !BAREBONES

using System;
using CommandSystem;
using LabApi.Features.Wrappers;

#pragma warning disable CS1591

namespace Callvote.API.Features.Commands.DefaultCommands
{
    internal class SecretLabCommand : ICommand
    {
        private readonly VoteCommand command;

        internal SecretLabCommand(VoteCommand command)
        {
            this.command = command;
        }

        public string Command => this.command.Command;

        public string[] Aliases => [];

        public string Description => this.command.Description;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            ReferenceHub player = sender is ServerConsoleSender ? Server.Host.ReferenceHub : Player.Get(sender)?.ReferenceHub;

            return this.command.OnCommandExecuted(player, out response);
        }
    }
}

#endif