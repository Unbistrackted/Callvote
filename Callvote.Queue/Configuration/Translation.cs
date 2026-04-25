#if EXILED
using Exiled.API.Interfaces;
#endif
using System.ComponentModel;

namespace Callvote.Queue.Configuration
{
#if EXILED
    public class Translation : ITranslation
#else
    public class Translation
#endif
    {
        public string QueueDisabled { get; set; } = "Callvote queue disabled.";

        public string QueueCleared { get; set; } = "Vote Queue Cleared.";

        public string QueuePaused { get; set; } = "Queue Paused";

        public string QueueResumed { get; set; } = "Vote Queue resumed.";

        public string RemovedFromQueue { get; set; } = "Removed %Number% Vote(s)";

        public string NoVoteInQueue { get; set; } = "There's no vote in the queue.";

        public string QueueIsFull { get; set; } = "<color=red>Queue is full.</color>";

        public string VoteEnqueued { get; set; } = "<color=#EDF193>Vote Enqueued.</color>";

        public string TypeNotFound { get; set; } = "Did not find any Vote with the type <color=red>%Type%</color>";
    }
}