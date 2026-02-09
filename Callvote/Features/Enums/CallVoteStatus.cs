using Callvote.API;

namespace Callvote.Features.Enums
{
    /// <summary>
    /// Represents the enumeration for the <see cref="VoteHandler.CallVote(Vote)"/> Status.
    /// </summary>
    public enum CallVoteStatus
    {
        /// <summary><see cref="Vote"/>  was enqueued.</summary>
        VoteEnqueued = -1,

        /// <summary><see cref="Vote"/>  has started.</summary>
        VoteStarted,

        /// <summary>There is a <see cref="Vote"/> currently in progress.</summary>
        VoteInProgress,

        /// <summary>The <see cref="VoteHandler.VoteQueue"/> is full.</summary>
        QueueIsFull,

        /// <summary>The <see cref="VoteHandler.VoteQueue"/> is disabled.</summary>
        QueueDisabled,

        /// <summary>The Player reached the maximum ammount of votes in a round.</summary>
        MaxedCallVotes,
    }
}
