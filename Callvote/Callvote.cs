using System;
using Callvote.VoteHandlers;
using Exiled.API.Enums;
using Exiled.API.Features;
using UserSettings.ServerSpecific;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using Exiled.API.Features.Core.UserSettings;

namespace Callvote
{
    public class Callvote : Plugin<Config, Translation>
    {
        public static Callvote Instance;
        public EventHandlers EventHandlers;

        public override string Name { get; } = AssemblyInfo.Name;
        public override string Author { get; } = AssemblyInfo.Author;
        public override Version Version { get; } = new Version(AssemblyInfo.Version);
        public override string Prefix { get; } = AssemblyInfo.LangFile;
        public override Version RequiredExiledVersion { get; } = new Version(9, 5, 1);
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public HeaderSetting SettingsHeader { get; set; } = new HeaderSetting("Callvote");

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers();
            Instance = this;
            SettingBase.Register(new[] { SettingsHeader });
            ServerSpecificSettings.RegisterSettings();
            Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded += EventHandlers.OnRoundEnded;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += VotingAPI.ProcessUserInput;
        }

        public override void OnDisabled()
        {
            Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded -= EventHandlers.OnRoundEnded;
            ServerSpecificSettings.UnregisterSettings();
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= VotingAPI.ProcessUserInput;
            SettingBase.Unregister(settings: new[] { SettingsHeader });
            Instance = null;
            EventHandlers = null;
            VotingAPI.CurrentVoting = null;
        }

        public override void OnReloaded()
        {
        }
    }
}