using System;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    internal class HelpCommand : ICommand
    {
        public string Command => "help";

        public string[] Aliases => new[] { "help" };

        public string Description => "Usage of Callvote.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "callvote custom <question> <option1> <option2>...." +
                "\ncallvote binary <question>" +
                "\ncallvote kick <player>" +
                "\ncallvote kill <player>" +
                "\ncallvote nuke" +
                "\ncallvote respawnwave" +
                "\ncallvote restartround" +
                "\ncallvote stopvote" +
                "\ncallvote rig <option>";
            return true;
        }
    }
}