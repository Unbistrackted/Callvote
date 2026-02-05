#if EXILED
using ServerEvents = Exiled.Events.Handlers.Server;
using RoundEndedEventArgs = Exiled.Events.EventArgs.Server.RoundEndedEventArgs;
using Exiled.API.Features;
#else
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
#endif
using UserSettings.ServerSpecific;
using Callvote.API;
using CommandSystem;

namespace Callvote
{
    public class EventHandlers
    {
        public EventHandlers()
        {
            ServerEvents.WaitingForPlayers += OnWaitingForPlayers;
            ServerEvents.RoundEnded += OnRoundEnded;
#if EXILED
            ServerEvents.RestartingRound += OnRoundRestarting;
#else
            ServerEvents.RoundRestarted += OnRoundRestarting;
#endif
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += OnUserInput;
        }
        ~EventHandlers()
        {
            ServerEvents.WaitingForPlayers -= OnWaitingForPlayers;
            ServerEvents.RoundEnded -= OnRoundEnded;
#if EXILED
            ServerEvents.RestartingRound -= OnRoundRestarting;
#else
            ServerEvents.RoundRestarted -= OnRoundRestarting;
#endif
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnUserInput;
        }

        public void OnWaitingForPlayers()
        {
            VotingHandler.Clear();
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            VotingHandler.Clear();
        }

        public void OnRoundRestarting()
        {
            VotingHandler.Clear();
        }

        public void OnUserInput(ReferenceHub sender, ServerSpecificSettingBase settingBase)
        {
            if (!VotingHandler.IsVotingActive)
                return;

            if (settingBase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                switch (keybindSetting.SettingId)
                {
                    case int id when id == Callvote.Instance.Config.YesKeybindSettingId:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(Callvote.Instance.Translation.CommandYes, out ICommand yesCommand))
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), yesCommand.Command);
                        break;

                    case int id when id == Callvote.Instance.Config.NoKeybindSettingId:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(Callvote.Instance.Translation.CommandNo, out ICommand noCommand))
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), noCommand.Command);
                        break;

                    case int id when id == Callvote.Instance.Config.MtfKeybindSettingId:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(Callvote.Instance.Translation.CommandMobileTaskForce, out ICommand mtfCommand))
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), mtfCommand.Command);
                        break;

                    case int id when id == Callvote.Instance.Config.CiKeybindSettingId:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(Callvote.Instance.Translation.CommandChaosInsurgency, out ICommand ciCommand))
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), ciCommand.Command);
                        break;
                }
            }
        }
    }
}
