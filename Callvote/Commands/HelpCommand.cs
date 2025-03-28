using CommandSystem;
using System;

namespace Callvote.Commands
{
    internal class HelpCommand : ICommand
    {
        public string Command => "help";

        public string[] Aliases => new[] { "help" };

        public string Description => "Usage of Callvote.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "callvote custom <question(detail) <option1> <option2>(detail2)...." +
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