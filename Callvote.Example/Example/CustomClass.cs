using Exiled.API.Features;
using System;

namespace CallNukeVoting
{
    public class CustomClass : Plugin<Config>
    {
        public static CustomClass Instance => Singleton;
        private static readonly CustomClass Singleton = new CustomClass();


        public override string Author { get; } = "Unbistrackted";
        public override string Name { get; } = "Example";
        public override string Prefix { get; } = "Example";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 5, 1);

        private EventHandler eventHandler;

        public override void OnEnabled()
        {
            eventHandler = new EventHandler();
            Exiled.Events.Handlers.Player.Died += eventHandler.OnDied;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            eventHandler = null;
            Exiled.Events.Handlers.Player.Died -= eventHandler.OnDied;
            base.OnDisabled();
        }

    }
}
