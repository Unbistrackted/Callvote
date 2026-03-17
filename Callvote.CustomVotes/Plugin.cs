using Callvote.CustomVotes.Configuration;
using Callvote.CustomVotes.Features;
using LabApi.Loader.Features.Plugins;
using System;

namespace Callvote.CustomVotes
{
    internal class Plugin : Plugin<Config>
    {
        private EventHandler eventHandler;

        public static Plugin Instance { get; private set; }

        public override string Name => typeof(Plugin).Assembly.GetName().Name;

        public override string Description => "Custom Votes Module for Callvote";

        public override string Author => "Unbistrackted";

        public override Version Version => typeof(Plugin).Assembly.GetName().Version;

        public override Version RequiredApiVersion => new(1, 1, 5);

        public Translation Translation { get; private set; }

        public override void Enable()
        {
            Instance = this;
            eventHandler = new EventHandler();
            SSSCustomVoteMenu.RegisterSettings();
        }

        public override void Disable()
        {
            SSSCustomVoteMenu.UnregisterSettings();
            Instance = null;
            eventHandler = null;
        }
    }
}
