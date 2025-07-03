using Callvote.API;
using Callvote.Features;
using CommandSystem;
using LabApi.Features.Wrappers;
using LabApi.Features.Permissions;
using System;
using System.Linq;
using Callvote.Commands.ParentCommands;

namespace Callvote.Commands
{
    [CommandHandler(typeof(CallVoteCommand))]
    public class TranslationCommand : ICommand
    {
        public string Command => "translation";

        public string[] Aliases => new[] { "ct", "changet", "changetranslation" };

        public string Description => "Change Callvote's Translation.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!player.HasPermissions("cv.translation") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (VotingHandler.CurrentVoting != null || VotingHandler.VotingQueue.Count > 0)
            {
                response = Callvote.Instance.Translation.VotingInProgress;
                return false;
            }

            string language = args.ElementAtOrDefault(0)?.ToLower() ?? string.Empty;

            if (!ChangeTranslation.LoadTranslation(language))
            {
                response = "Something went wrong. Please check the server console.";
                return false;
            }

            response = Callvote.Instance.Translation.TranslationChanged;
            return true;
        }
    }
}
