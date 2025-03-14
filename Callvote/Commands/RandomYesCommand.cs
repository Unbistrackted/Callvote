using System;
using CommandSystem;
using Exiled.API.Features;

namespace Callvote.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class YesLMAO : ICommand
    {

        public string Command => "yes";

        public string[] Aliases => new[] { "y" };

        public string Description => "Enables player to call votings!";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            response = "Wrong Syntax, use .callvote help";
            return false;
        }
    }
}