#if EXILED
using LoadPriority = Exiled.API.Enums.PluginPriority;
using Plugin = Exiled.API.Features.Plugin<Callvote.Queue.Configuration.Config, Callvote.Queue.Configuration.Translation>;
#else
using System.Reflection;
using Callvote.Queue.Configuration;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
#endif
using System;
using Callvote.API.Features.Commands;
using Callvote.Commands.ParentCommands;
using HarmonyLib;
using MEC;

namespace Callvote.Queue
{
    public class QueuePlugin : Plugin
    {
        private EventHandler eventHandler;

        public static QueuePlugin Instance { get; private set; }

        public override string Name { get; } = typeof(QueuePlugin).Assembly.GetName().Name;

        public override Version Version { get; } = typeof(QueuePlugin).Assembly.GetName().Version;

        public override string Author => "Unbistrackted";

        public override LoadPriority Priority => LoadPriority.Lowest;

#if EXILED
        public override Version RequiredExiledVersion => new(9, 13, 1);

#else
        public override string Description => typeof(QueuePlugin).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        public override Version RequiredApiVersion => new Version(LabApi.Features.LabApiProperties.CompiledVersion);

        public Harmony Harmony { get; private set; } = new("Callvote.Queue");

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
            this.eventHandler = new EventHandler();
            this.Harmony.PatchAll();

            Timing.RunCoroutine(PluginLoadCommandRegister.RegisterCommands(typeof(QueuePlugin).Assembly, typeof(CallVoteParentCommand), true), Segment.FixedUpdate);
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
            this.Harmony.UnpatchAll();
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