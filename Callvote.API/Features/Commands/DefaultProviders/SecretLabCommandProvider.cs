using Callvote.API.Features.Commands.DefaultCommands;
using CommandSystem;
using RemoteAdmin;

namespace Callvote.API.Features.Commands.DefaultProviders
{
    public class SecretLabCommandProvider : CommandProvider
    {
        public override string Name => "SecretLabCommandProvider";

        public override bool IsCommandRegistered(VoteCommand command)
        {
            return QueryProcessor.DotCommandHandler.TryGetCommand(command.Command, out _);
        }

        public override void RegisterCommand(VoteCommand command)
        {
            if (this.IsCommandRegistered(command))
            {
                command.Command = "cv" + command.Command;
            }

            QueryProcessor.DotCommandHandler.RegisterCommand(new SecretLabCommand(command));
        }

        public override void UnregisterCommand(VoteCommand command)
        {
            if (!QueryProcessor.DotCommandHandler.TryGetCommand(command.Command, out ICommand cmd))
            {
                return;
            }

            QueryProcessor.DotCommandHandler.UnregisterCommand(cmd);
        }
    }
}
