#if EXILED
using Exiled.API.Enums;
using Plugin = Exiled.API.Features.Plugin<Callvote.Configuration.Config, Callvote.Configuration.Translation>;
#else
using Callvote.Configuration;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
#endif
using System;
using System.ComponentModel;
using Callvote.API.Features.Displays;
using Callvote.Features;
using Callvote.Properties;
using HarmonyLib;
using LabApi.Features.Console;

namespace Callvote
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only public API documentation is required")]
    public class CallvotePlugin : Plugin
    {
        private const string CallvoteLogo = """

==================================================
         █████                                      
        ███████                                     
   ██   ███████                       █████        
  ████    ███                       █      █         
  █   ███████████              ██  █      █          
 ██  █████████████          ███   █      █  ███     
 ██                      ███       ██████      ███  
███████████████████████  █      -----------      █  
██████           ██████  █████               █████  
      ███████████        █    ████       ████    █  
      ███████████        █        ███████        █  
      ███████████        █           █           █  
      ███████████         ███        █         ██   
      ███████████            ████    █     ████     
      ███████████                ██████████         
      ███████████                                   
      ███████████                                   
      ███████████                                   

Version: 7.0.0 - RELEASE CANDIDATE II
!!! REPORT ANY BUGS TO @Unbistrackted !!!
==================================================
""";

        private EventHandlers eventHandler;

        public static CallvotePlugin Instance { get; private set; }

        public override string Name { get; } = AssemblyInfo.Name;

        public override string Author { get; } = AssemblyInfo.Author;

#if EXILED
        public override Version RequiredExiledVersion => new(9, 13, 1);

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

        public Harmony Harmony { get; private set; } = new("CallvotePlugin");

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
            this.eventHandler = new EventHandlers();
            SSSCallvoteMenu.RegisterSettings();
            this.Harmony.PatchAll();
            DisplayHandler.Instance.RegisterProvider(SoftDependencies.DisplayMessageHandler.GetMessageProvider());
            if (this.Config.ShowLogo)
            {
                Logger.Raw($"\n{CallvoteLogo.Trim('\n')}\n", ConsoleColor.DarkGreen);
            }
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
            SSSCallvoteMenu.UnregisterSettings();
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