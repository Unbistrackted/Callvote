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

            if (!player.CheckPermission("cv.translation"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote translation <countryCode>";
                return false;
            }

            ChangeTranslation.LoadTranslation(args.At(0));

            response = Callvote.Instance.Translation.TranslationChanged;
            return true;
        }
    }
}
