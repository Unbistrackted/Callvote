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
using CommandSystem;
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

        private void OnWaitingForPlayers()
        {
            VotingHandler.Clear();
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            VotingHandler.Clear();
        }

        private void OnRoundRestarting()
        {
            VotingHandler.Clear();
        }

        private void OnUserInput(ReferenceHub sender, ServerSpecificSettingBase settingBase)
        {
            if (!VotingHandler.IsVotingActive)
            {
                return;
            }

            if (settingBase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                switch (keybindSetting.SettingId)
                {
                    case int id when id == CallvotePlugin.Instance.Config.YesKeybindSettingId:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(CallvotePlugin.Instance.Translation.CommandYes, out ICommand yesCommand))
                        {
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), yesCommand.Command);
                        }

                        break;

                    case int id when id == CallvotePlugin.Instance.Config.NoKeybindSettingId:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(CallvotePlugin.Instance.Translation.CommandNo, out ICommand noCommand))
                        {
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), noCommand.Command);
                        }

                        break;

                    case int id when id == CallvotePlugin.Instance.Config.MtfKeybindSettingId:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(CallvotePlugin.Instance.Translation.CommandMobileTaskForce, out ICommand mtfCommand))
                        {
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), mtfCommand.Command);
                        }

                        break;

                    case int id when id == CallvotePlugin.Instance.Config.CiKeybindSettingId:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(CallvotePlugin.Instance.Translation.CommandChaosInsurgency, out ICommand ciCommand))
                        {
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), ciCommand.Command);
                        }

                        break;
                }
            }
        }
    }
}
