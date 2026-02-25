#if !EXILED
using Callvote.Commands.ParentCommands;
#endif
using System;
using CommandSystem;

namespace Callvote.Commands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteParentCommand))]
#endif
    public class HelpCommand : ICommand
    {
        public string Command => "help";

        public string[] Aliases => [];

        public string Description => "Usage of Callvote.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response =
                "\ncallvote custom [\"question\"] [command1(detail1)] [command2(detail2)]...." +
                "\ncallvote binary [question]" +
                "\ncallvote kick [player] [reason]" +
                "\ncallvote kill [player] [reason]" +
                "\ncallvote nuke" +
                "\ncallvote respawnwave" +
                "\ncallvote restartround" +
                "\ncallvote stopvote" +
                "\ncallvote ff" +
                "\ncallvote queue [check/clear/pause/removeplayer/removetype/removeindex] [player/type/index]" +
                "\ncallvote rig [option]" +
                "\ncallvote translation [none/language/countryCode]" +
                "\n<color=red>REMOVE THE SQUARE BRACKETS (-> []) WHEN USING THE COMMAND</color>";
            return true;
        }
    }
}