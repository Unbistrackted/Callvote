using Callvote.API;
using Callvote.Configuration;
using Callvote.Features;
using System;
using LabApi;
using LabApi.Loader.Features.Plugins;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Handlers;
using LabApi.Loader;
using UserSettings.ServerSpecific;

namespace Callvote
{
    public class Callvote : Plugin
    {
        private EventHandlers EventHandlers;

        public static Callvote Instance;
        public override string Name { get; } = AssemblyInfo.Name;
        public override string Author { get; } = AssemblyInfo.Author;
        public override string Description => AssemblyInfo.Description;
        public override Version Version { get; } = Version.Parse(AssemblyInfo.Version);
        public string Prefix { get; } = AssemblyInfo.LangFile;
        public override Version RequiredApiVersion { get; } = new Version(1, 1, 0);
        public Translation Translation { get; private set; }
        public Config Config { get; private set; }

        public override void Enable()
        {
            Instance = this;
            LoadConfigs();
            RegisterEvents();
        }

        public override void Disable()
        {
            UnregisterEvents();
            Instance = null;
        }

        private void RegisterEvents()
        {
            EventHandlers = new EventHandlers();
            ServerSpecificSettings.RegisterSettings();
            ServerEvents.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            ServerEvents.RoundEnded += EventHandlers.OnRoundEnded;
            ServerEvents.RoundRestarted += EventHandlers.OnRoundRestarted;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += EventHandlers.OnUserInput;
        }

        private void UnregisterEvents()
        {
            ServerSpecificSettings.UnregisterSettings();
            ServerEvents.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            ServerEvents.RoundEnded -= EventHandlers.OnRoundEnded;
            ServerEvents.RoundRestarted -= EventHandlers.OnRoundRestarted;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= EventHandlers.OnUserInput;
            VotingHandler.Clear();
            EventHandlers = null;
        }

        public override void LoadConfigs()
        {
            this.TryLoadConfig("config.yml", out Config config);
            Config = config ?? new Config();
            this.TryLoadConfig("translation.yml", out Translation translation);
            Translation = translation ?? new Translation();
        }
    }
}