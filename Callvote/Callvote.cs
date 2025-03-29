using Callvote.VoteHandlers;
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

        public static Callvote Instance;
        public EventHandlers EventHandlers;

        public override string Name { get; } = AssemblyInfo.Name;
        public override string Author { get; } = AssemblyInfo.Author;
        public override Version Version { get; } = Version.Parse(AssemblyInfo.Version);
        public override string Prefix { get; } = AssemblyInfo.LangFile;
        public override Version RequiredExiledVersion { get; } = new Version(9, 5, 1);
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public HeaderSetting SettingsHeader { get; set; } = new HeaderSetting(AssemblyInfo.Name);

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers();
            Callvote.Instance = this;
            SettingBase.Register(new[] { SettingsHeader });
            ServerSpecificSettings.RegisterSettings();
            Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded += EventHandlers.OnRoundEnded;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += VotingHandler.ProcessUserInput;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded -= EventHandlers.OnRoundEnded;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= VotingHandler.ProcessUserInput;
            ServerSpecificSettings.UnregisterSettings();
            SettingBase.Unregister(settings: new[] { SettingsHeader });
            Callvote.Instance = null;
            EventHandlers = null;
            base.OnDisabled();
        }
    }
}