#if EXILED
using Exiled.API.Features;
using RoundEndedEventArgs = Exiled.Events.EventArgs.Server.RoundEndedEventArgs;
using ServerEvents = Exiled.Events.Handlers.Server;
#else
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
#endif
using Callvote.API;
using Callvote.Configuration;
using Callvote.Features;
using UserSettings.ServerSpecific;

namespace Callvote
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only public API documentation is required")]
    internal class EventHandlers
    {
        internal EventHandlers()
        {
            ServerEvents.WaitingForPlayers += this.OnWaitingForPlayers;
            ServerEvents.RoundEnded += this.OnRoundEnded;
#if EXILED
            ServerEvents.RestartingRound += this.OnRoundRestarting;
#else
            ServerEvents.RoundRestarted += this.OnRoundRestarting;
#endif
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += this.OnUserInput;
        }

        ~EventHandlers()
        {
            ServerEvents.WaitingForPlayers -= this.OnWaitingForPlayers;
            ServerEvents.RoundEnded -= this.OnRoundEnded;
#if EXILED
            ServerEvents.RestartingRound -= this.OnRoundRestarting;
#else
            ServerEvents.RoundRestarted -= this.OnRoundRestarting;
#endif
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= this.OnUserInput;
        }

        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        private void OnWaitingForPlayers()
        {
            VoteHandler.Clear();
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            VoteHandler.Clear();
        }

        private void OnRoundRestarting()
        {
            VoteHandler.Clear();
        }

        private void OnUserInput(ReferenceHub sender, ServerSpecificSettingBase settingBase)
        {
            if (!VoteHandler.IsVoteActive || !Config.EnableSSMenu)
            {
                return;
            }

            if (settingBase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                switch (keybindSetting.SettingId)
                {
                    case int id when id == Config.YesKeybindSettingId:
                        if (VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(Translation.CommandYes, out VoteOption yesVote))
                        {
                            VoteHandler.CurrentVote.SubmitVoteOption(Player.Get(sender), yesVote);
                        }

                        break;

                    case int id when id == Config.NoKeybindSettingId:
                        if (VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(Translation.CommandNo, out VoteOption noVote))
                        {
                            VoteHandler.CurrentVote.SubmitVoteOption(Player.Get(sender), noVote);
                        }

                        break;

                    case int id when id == Config.MtfKeybindSettingId:
                        if (VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(Translation.CommandMobileTaskForce, out VoteOption mtfVote))
                        {
                            VoteHandler.CurrentVote.SubmitVoteOption(Player.Get(sender), mtfVote);
                        }

                        break;

                    case int id when id == Config.CiKeybindSettingId:
                        if (VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(Translation.CommandChaosInsurgency, out VoteOption ciVote))
                        {
                            VoteHandler.CurrentVote.SubmitVoteOption(Player.Get(sender), ciVote);
                        }

                        break;
                }
            }
        }
    }
}