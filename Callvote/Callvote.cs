using Callvote.API;
using Callvote.Configuration;
using Callvote.Features;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using System;
using UserSettings.ServerSpecific;
using Server = Exiled.Events.Handlers.Server;

namespace Callvote
{
    public class Callvote : Plugin<Config, Translation>
    {
        private EventHandlers EventHandlers;

        public static Callvote Instance;
        public override string Name { get; } = AssemblyInfo.Name;
        public override string Author { get; } = AssemblyInfo.Author;
        public override Version Version { get; } = Version.Parse(AssemblyInfo.Version);
        public override string Prefix { get; } = AssemblyInfo.LangFile;
        public override Version RequiredExiledVersion { get; } = new Version(9, 5, 1);
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public HeaderSetting SettingsHeader { get; set; } = new HeaderSetting(AssemblyInfo.Name);

        public override void OnEnabled()
        {
            Instance = this;
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            VotingHandler.Clear();
            base.OnDisabled();
            Instance = null;
        }

        private void RegisterEvents()
        {
            EventHandlers = new EventHandlers();
            SettingBase.Register(new[] { SettingsHeader });
            ServerSpecificSettings.RegisterSettings();
            Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded += EventHandlers.OnRoundEnded;
            Server.RestartingRound += EventHandlers.OnRoundRestarting;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += EventHandlers.OnUserInput;
        }

        private void UnregisterEvents()
        {
            ServerSpecificSettings.UnregisterSettings();
            SettingBase.Unregister(settings: new[] { SettingsHeader });
            Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded -= EventHandlers.OnRoundEnded;
            Server.RestartingRound -= EventHandlers.OnRoundRestarting;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= EventHandlers.OnUserInput;
            EventHandlers = null;
        }
    }
}