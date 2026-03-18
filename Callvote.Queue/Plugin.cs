using System;
using System.Reflection;
using Callvote.Queue.Configuration;
using LabApi.Loader.Features.Plugins;

namespace Callvote.Queue
{
    public class Plugin : Plugin<Config>
    {
        private EventHandler eventHandler;

        public static Plugin Instance { get; private set; }

        public override string Name => typeof(Plugin).Assembly.GetName().Name;

        public override string Description => typeof(Plugin).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        public override string Author => "Unbistrackted";

        public override Version Version => typeof(Plugin).Assembly.GetName().Version;

        public override Version RequiredApiVersion => new(LabApi.Features.LabApiProperties.CompiledVersion);

        public override void Enable()
        {
            Instance = this;
            this.eventHandler = new EventHandler();
        }

        public override void Disable()
        {
            this.eventHandler = null;
            Instance = null;
        }
    }
}
