using Callvote.API.Features.Votes;

namespace Callvote.API.Events.Interfaces
{
    /// <summary>
    /// Event args used for all <see cref="Features.Votes.VoteOption"/> related events.
    /// </summary>
    public interface IVoteOptionEvent
    {
        /// <summary>
        /// Gets the <see cref="Features.Votes.VoteOption"/> related to the event.
        /// </summary>
        VoteOption VoteOption { get; }
    }
}
