#if EXILED
using Exiled.API.Features;
using LoadPriority = Exiled.API.Enums.PluginPriority;
#else
using System.Reflection;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
#endif
using System;
using Callvote.ScpDiscord.Configuration;

namespace Callvote.ScpDiscord
{
    public class Plugin : Plugin<Config>
    {
        private EventHandler eventHandler;

        public static Plugin Instance { get; private set; }

        public override string Name { get; } = typeof(Plugin).Assembly.GetName().Name;

        public override Version Version { get; } = typeof(Plugin).Assembly.GetName().Version;

        public override string Author => "Unbistrackted";

        public override LoadPriority Priority => LoadPriority.Lowest;

#if EXILED
        public override Version RequiredExiledVersion => new(9, 13, 1);

#else
        public override string Description => typeof(Plugin).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        public override Version RequiredApiVersion => new Version(LabApi.Features.LabApiProperties.CompiledVersion);

#endif

#if EXILED
        public override void OnEnabled()
#else
        public override void Enable()
#endif
        {
            Instance = this;
            this.eventHandler = new EventHandler();

#if EXILED
            base.OnEnabled();
#endif
        }

#if EXILED
        public override void OnDisabled()
#else
        public override void Disable()
#endif
        {
            this.eventHandler = null;
            Instance = null;

#if EXILED
            base.OnDisabled();
#endif
        }
    }
}