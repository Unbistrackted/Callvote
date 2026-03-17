using Callvote.DiscordLab.Configuration;
using LabApi.Loader.Features.Plugins;
using System;

namespace Callvote.DiscordLab
{
    public class Plugin : Plugin<Config>
    {
        private EventHandler eventHandler;

        public static Plugin Instance { get; private set; }

        public override string Name => typeof(Plugin).Assembly.GetName().Name;

        public override string Description => "DiscordLab Module for Callvote";

        public override string Author => "Unbistrackted";

        public override Version Version => typeof(Plugin).Assembly.GetName().Version;

        public override Version RequiredApiVersion => new(1, 1, 5);


        public override void Enable()
        {
            Instance = this;
            eventHandler = new EventHandler();
        }

        public override void Disable()
        {
            eventHandler = null;
            Instance = null;
        }
    }
}
