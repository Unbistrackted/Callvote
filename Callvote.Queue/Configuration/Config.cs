#if EXILED
using Exiled.API.Interfaces;
#endif
using System.ComponentModel;

namespace Callvote.Queue.Configuration
{
#if EXILED
    public class Config : IConfig
#else
    public class Config
#endif
    {
#if EXILED
        [Description("Is the plugin enabled")]
        public bool IsEnabled { get; set; } = true;

#endif
        [Description("Are the plugin's debug logs enabled")]
        public bool Debug { get; set; } = false;

        [Description("Enables or disables the Queue.")]
        public bool EnableQueue { get; set; } = true;

        [Description("Changes Callvote's Queue size if enabled.")]
        public int QueueSize { get; set; } = 5;

        [Description("Changes the maximum amount of vote each player can call in a match.")]
        public int MaxAmountOfVotesPerRound { get; set; } = 10;
    }
}
