#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using Callvote.Commands.ParentCommands;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using System.Linq;
using System.Threading.Tasks;
using Callvote.API;
using Callvote.Features;
using CommandSystem;
using MEC;

namespace Callvote.Commands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteCommand))]
#endif
    public class TranslationCommand : ICommand
    {
        public string Command => "translation";

        public string[] Aliases => new[] { "ct", "changet", "changetranslation" };

        public string Description => "Change Callvote's Translation.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
#if EXILED
            if (!player.CheckPermission("cv.translation") && player != null)
#else
            if (!player.HasPermissions("cv.translation") && player != null)
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (VotingHandler.IsVotingActive || VotingHandler.VotingQueue.Count > 0)
            {
                response = CallvotePlugin.Instance.Translation.VotingInProgress;
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

                    player.SendConsoleMessage(CallvotePlugin.Instance.Translation.TranslationChanged, "green");
                });
            });

            response = "Please wait and check your console in a few seconds.";

            response = CallvotePlugin.Instance.Translation.TranslationChanged;
            return true;
        }
    }
}
