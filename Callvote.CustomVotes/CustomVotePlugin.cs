#if EXILED
using LoadPriority = Exiled.API.Enums.PluginPriority;
using Plugin = Exiled.API.Features.Plugin<Callvote.CustomVotes.Configuration.Config, Callvote.CustomVotes.Configuration.Translation>;
#else
using System.Reflection;
using Callvote.CustomVotes.Configuration;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
#endif
using System;
using Callvote.API.Features.Commands;
using Callvote.Commands.ParentCommands;
using Callvote.CustomVotes.Features;
using MEC;

namespace Callvote.CustomVotes
{
    public class CustomVotePlugin : Plugin
    {
        private EventHandler eventHandler;

        public static CustomVotePlugin Instance { get; private set; }

        public override string Name { get; } = typeof(CustomVotePlugin).Assembly.GetName().Name;

        public override Version Version { get; } = typeof(CustomVotePlugin).Assembly.GetName().Version;

        public override string Author => "Unbistrackted";

        public override LoadPriority Priority => LoadPriority.Lowest;

#if EXILED
        public override Version RequiredExiledVersion => new(9, 13, 1);

#else
        public override string Description => typeof(CustomVotePlugin).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        public override Version RequiredApiVersion => new Version(LabApi.Features.LabApiProperties.CompiledVersion);

        public Translation Translation { get; private set; }

        public Config Config { get; private set; }
#endif

#if EXILED
        public override void OnEnabled()
#else
        public override void Enable()
#endif
        {
            Instance = this;

#if !EXILED
            this.LoadConfigs();
#endif

            this.eventHandler = new EventHandler();
            SSSCustomVoteMenu.RegisterSettings();
            Timing.RunCoroutine(PluginLoadCommandRegister.RegisterCommands(typeof(CustomVotePlugin).Assembly, typeof(CallVoteParentCommand), false));
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
            SSSCustomVoteMenu.UnregisterSettings();
            this.eventHandler = null;
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
            this.Config = config ?? new Config();
            this.Translation = translation ?? new Translation();
        }
#endif
    }
}