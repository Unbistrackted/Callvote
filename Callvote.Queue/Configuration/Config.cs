using System.ComponentModel;

namespace Callvote.Queue.Configuration
{
    public class Config
    {
        [Description("Enables or disables the Queue.")]
        public bool EnableQueue { get; set; } = true;

        [Description("Changes Callvote's Queue size if enabled.")]
        public int QueueSize { get; set; } = 5;

        [Description("Changes the maximum amount of vote each player can call in a match.")]
        public int MaxAmountOfVotesPerRound { get; set; } = 10;
    }
}
