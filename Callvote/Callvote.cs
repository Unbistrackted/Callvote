#pragma warning disable IDE0052

using Callvote.API;
using Callvote.Configuration;
using Callvote.Features;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using System;

namespace Callvote
{
    public class Callvote : Plugin
    {
        public static Callvote Instance;
        public override string Name { get; } = AssemblyInfo.Name;
        public override string Author { get; } = AssemblyInfo.Author;
        public override string Description => AssemblyInfo.Description;
        public override Version Version { get; } = Version.Parse(AssemblyInfo.Version);
        public string Prefix { get; } = AssemblyInfo.LangFile;
        public override Version RequiredApiVersion { get; } = new Version(1, 1, 4);
        public Translation Translation { get; private set; }
        public Config Config { get; private set; }
        private EventHandlers EventHandlers;

        public override void Enable()
        {
            Instance = this;
            LoadConfigs();
            EventHandlers = new EventHandlers();
            ServerSpecificSettings.RegisterSettings();
        }

        public override void Disable()
        {
            ServerSpecificSettings.UnregisterSettings();
            EventHandlers = null;
            Instance = null;
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