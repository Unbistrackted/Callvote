using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallNukeVoting
{
    public class CallNukeVoting : Plugin<Config>
    {
        public static CallNukeVoting Instance => Singleton;
        public override string Author { get; } = "Unbistrackted";
        public override string Name { get; } = "CallNukeVoting";
        public override string Prefix { get; } = "CallNukeVoting";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 5, 1);

        private static readonly CallNukeVoting Singleton = new CallNukeVoting();
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
