using Callvote.Commands.ParentCommands;
using CommandSystem;
using System;

namespace Callvote.Commands
{
    [CommandHandler(typeof(CallVoteCommand))]
    public class HelpCommand : ICommand
    {
        public string Command => "help";

        public string[] Aliases => new[] { "help" };

        public string Description => "Usage of Callvote.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response =
                "\ncallvote custom <question(detail) <option1> <option2>(detail2)...." +
                "\ncallvote binary <question>" +
                "\ncallvote kick <player>" +
                "\ncallvote kill <player>" +
                "\ncallvote nuke" +
                "\ncallvote respawnwave" +
                "\ncallvote restartround" +
                "\ncallvote stopvote" +
                "\ncallvote ff" +
                "\ncallvote queue <check/clear/pause/removeplayer (player)/removetype (type)/removeindex (index)>" +
                "\ncallvote rig <option>" +
                "\ncallvote translation <countryCode>";
            return true;
        }
    }
}