using Callvote.DiscordLab.Configuration;
using LabApi.Loader.Features.Plugins;
using System;

namespace Callvote.DiscordLab
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }

        public override string Name => throw new NotImplementedException();

        public override string Description => throw new NotImplementedException();

        public override string Author => throw new NotImplementedException();

        public override Version Version => throw new NotImplementedException();

        public override Version RequiredApiVersion => throw new NotImplementedException();

        public override void Disable()
        {
            throw new NotImplementedException();
        }

        public override void Enable()
        {
            Instance = this;
            throw new NotImplementedException();
        }
    }
}
