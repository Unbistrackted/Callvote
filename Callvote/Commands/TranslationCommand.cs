﻿using Callvote.API;
using Callvote.Features;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            if (VotingHandler.CurrentVoting != null || VotingHandler.VotingQueue.Count > 0)
            {
                response = Callvote.Instance.Translation.VotingInProgress;
                return false;
            }

            string language = args.ElementAtOrDefault(0)?.ToLower() ?? string.Empty;

            Task.Run(async () =>
            {
                bool result = await ChangeTranslation.LoadTranslation(language);

                Timing.CallDelayed(0f, () =>
                {
                    if (!result)
                    {
                        player.SendConsoleMessage("Something went wrong. Please check the server console.", "red");
                        return;
                    }

                    player.SendConsoleMessage(Callvote.Instance.Translation.TranslationChanged, "green");
                });
            });

            response = "Please wait and check your console in a few seconds.";
            return true;
        }
    }
}
