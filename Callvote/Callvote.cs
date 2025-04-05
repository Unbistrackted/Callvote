using Callvote.API;
using System;
using LabApi.Features.Console;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using LabApi.Loader;
using LabApi.Events.CustomHandlers;

namespace Callvote
{
    public class Callvote : Plugin<Config>
    {
        public static Callvote Instance;
        public new Config Config;
        public Translation Translation;
        public EventHandlers EventHandlers;

        public override string Name { get; } = AssemblyInfo.Name;
        public override string Author { get; } = AssemblyInfo.Author;
        public override string Description { get; } = AssemblyInfo.Description;
        public override Version Version { get; } = Version.Parse(AssemblyInfo.Version);
        public override Version RequiredApiVersion { get; } = new Version(0, 5, 0);
        public string ConfigFileName { get; set; } = "config.yml";
        public string TranslationFileName { get; set; } = "translation.yml";
        public override LoadPriority Priority { get; } = LoadPriority.Medium;
        //public HeaderSetting SettingsHeader { get; set; } = new HeaderSetting(AssemblyInfo.Name);


        private bool _configHasIncorrectSettings = false;
        private bool _translationHasIncorrectSettings = false;

        public override void Enable()
        {
            if (_configHasIncorrectSettings)
            {
                Logger.Error("Detected incorrect settings, not loading");
                return;
            }
            if (_translationHasIncorrectSettings)
            {
                Logger.Error("Detected incorrect settings, not loading");
                return;
            }
            this.SaveConfig(Config, ConfigFileName);
            this.SaveConfig(Translation, TranslationFileName);
            EventHandlers = new EventHandlers();
            Instance = this;
            VotingHandler.Init();
            CustomHandlersManager.RegisterEventsHandler(EventHandlers);
        }

        public override void Disable()
        {
            CustomHandlersManager.UnregisterEventsHandler(EventHandlers);
            VotingHandler.Clean();
            Instance = null;
            EventHandlers = null;
        }

        public override void LoadConfigs()
        {
            base.LoadConfigs();
            //_configHasIncorrectSettings = !this.TryLoadConfig(ConfigFileName, out Config);
            //_translationHasIncorrectSettings = !this.TryLoadConfig(TranslationFileName, out Translation);
            Translation = this.LoadConfig<Translation>(TranslationFileName);
            Config = this.LoadConfig<Config>(ConfigFileName);
        }
    }
}