﻿using Callvote.Features;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

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
