#if EXILED
using Plugin = Exiled.API.Features.Plugin<Callvote.Configuration.Config, Callvote.Configuration.Translation>;
using Exiled.API.Enums;
# else
using LabApi.Loader.Features.Plugins;
using LabApi.Loader;
using Callvote.Configuration;
#endif
using Callvote.Features;
using System;

namespace Callvote
{
    public class Callvote : Plugin
    {
        public static Callvote Instance;
        public override string Name { get; } = AssemblyInfo.Name;
        public override string Author { get; } = AssemblyInfo.Author;
#if EXILED
        public override Version RequiredExiledVersion => new(9, 12, 6);
        public override PluginPriority Priority => PluginPriority.Default;
        public override string Prefix { get; } = AssemblyInfo.LangFile;
#else
        public override string Description => AssemblyInfo.Description;
        public override Version RequiredApiVersion { get; } = new Version(1, 1, 5);
        public string Prefix { get; } = AssemblyInfo.LangFile;
        public Translation Translation { get; private set; }
        public Config Config { get; private set; }
#endif
        public override Version Version { get; } = Version.Parse(AssemblyInfo.Version);

        private EventHandlers _eventHandler;

#if EXILED
        public override void OnEnabled()
#else
        public override void Enable()
#endif
        {
            Instance = this;

#if !EXILED
            LoadConfigs();
#endif
            _eventHandler = new EventHandlers();
            ServerSpecificSettings.RegisterSettings();

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
            ServerSpecificSettings.UnregisterSettings();
            _eventHandler = null;
            Instance = null;

#if EXILED
            base.OnDisabled();
#endif
        }

#if !EXILED
        public override void LoadConfigs()
        {
            this.TryLoadConfig("config.yml", out Config config);
            this.TryLoadConfig("translation.yml", out Translation translation);
            Config = config ?? new Config();
            Translation = translation ?? new Translation();
        }
#endif
    }
}