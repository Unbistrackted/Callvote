using Callvote.Features;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace Callvote.Commands
{
    public class TranslationCommand : ICommand
    {
        public string Command => "translation";

        public string[] Aliases => new[] { "ct", "changet", "changetranslation" };

        public string Description => "Change Callvote's Translation.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!player.CheckPermission("cv.translation") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                ChangeTranslation.LoadTranslation("auto");
                response = Callvote.Instance.Translation.TranslationChanged;
                return true;
            }

            ChangeTranslation.LoadTranslation(args.At(0).ToUpper());

            response = Callvote.Instance.Translation.TranslationChanged;
            return true;
        }
    }
}
