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
                    case int id when id == Config.YesKeybindSettingId:
                        if (VotingHandler.CurrentVoting.TryGetVote(Translation.CommandYes, out Vote yesVote))
                        {
                            VotingHandler.CurrentVoting.VoteOption(Player.Get(sender), yesVote);
                        }

                        break;

                    case int id when id == Config.NoKeybindSettingId:
                        if (VotingHandler.CurrentVoting.TryGetVote(Translation.CommandNo, out Vote noVote))
                        {
                            VotingHandler.CurrentVoting.VoteOption(Player.Get(sender), noVote);
                        }

                        break;

                    case int id when id == Config.MtfKeybindSettingId:
                        if (VotingHandler.CurrentVoting.TryGetVote(Translation.CommandMobileTaskForce, out Vote mtfVote))
                        {
                            VotingHandler.CurrentVoting.VoteOption(Player.Get(sender), mtfVote);
                        }

                        break;

                    case int id when id == Config.CiKeybindSettingId:
                        if (VotingHandler.CurrentVoting.TryGetVote(Translation.CommandChaosInsurgency, out Vote ciVote))
                        {
                            VotingHandler.CurrentVoting.VoteOption(Player.Get(sender), ciVote);
                        }

                        break;
                }
            }
        }
    }
}
