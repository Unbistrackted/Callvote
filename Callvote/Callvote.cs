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
        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 1);
        public override PluginPriority Priority { get; } = PluginPriority.Default;

        public override void OnEnabled()
        {
            Instance = this;
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            base.OnDisabled();
            Instance = null;
        }

        private void RegisterEvents()
        {
            EventHandlers = new EventHandlers();
            ServerSpecificSettings.RegisterSettings();
            Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded += EventHandlers.OnRoundEnded;
            Server.RestartingRound += EventHandlers.OnRoundRestarting;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += EventHandlers.OnUserInput;
        }

        private void UnregisterEvents()
        {
            ServerSpecificSettings.UnregisterSettings();
            Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded -= EventHandlers.OnRoundEnded;
            Server.RestartingRound -= EventHandlers.OnRoundRestarting;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= EventHandlers.OnUserInput;
            VotingHandler.Clear();
            EventHandlers = null;
        }
    }
}