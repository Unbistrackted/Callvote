using System;
using Callvote.VoteHandlers;
using Exiled.API.Enums;
using Exiled.API.Features;
using Server = Exiled.Events.Handlers.Server;

namespace Callvote
{
    public class Plugin : Plugin<Config, Translation>
    {
        public static Plugin Instance;
        public EventHandlers EventHandlers;

        public override string Name { get; } = AssemblyInfo.Name;
        public override string Author { get; } = AssemblyInfo.Author;
        public override Version Version { get; } = new Version(AssemblyInfo.Version);
        public override string Prefix { get; } = AssemblyInfo.LangFile;
        public override Version RequiredExiledVersion { get; } = new Version(9, 5, 1);
        public override PluginPriority Priority { get; } = PluginPriority.Default;


        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers();
            Instance = this;
            Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded += EventHandlers.OnRoundEnded;
        }

        public override void OnDisabled()
        {
            Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded -= EventHandlers.OnRoundEnded;
            Instance = null;
            EventHandlers = null;
            VoteAPI.CurrentVoting = null;
        }

        public override void OnReloaded()
        {
        }
    }
}