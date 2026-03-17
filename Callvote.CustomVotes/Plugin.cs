using System;
using System.Reflection;
using Callvote.CustomVotes.Configuration;
using Callvote.CustomVotes.Features;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;

namespace Callvote.CustomVotes
{
    internal class Plugin : Plugin<Config>
    {
        private EventHandler eventHandler;

        public static Plugin Instance { get; private set; }

        public override string Name => typeof(Plugin).Assembly.GetName().Name;

        public override string Description => typeof(Plugin).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        public override string Author => "Unbistrackted";

        public override Version Version => typeof(Plugin).Assembly.GetName().Version;

        public override Version RequiredApiVersion => new(LabApi.Features.LabApiProperties.CompiledVersion);

        public override LoadPriority Priority => LoadPriority.Lowest;

        public Translation Translation { get; private set; }

        public override void Enable()
        {
            Instance = this;
            this.eventHandler = new EventHandler();
            SSSCustomVoteMenu.RegisterSettings();
        }

        public override void Disable()
        {
            SSSCustomVoteMenu.UnregisterSettings();
            Instance = null;
            this.eventHandler = null;
        }
    }
}
