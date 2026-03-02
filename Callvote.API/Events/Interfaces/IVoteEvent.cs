using System.Collections.Generic;
using Callvote.API.Features.Votes;

namespace Callvote.API.Events.Interfaces
{
    /// <summary>
    /// Event args used for all <see cref="Features.Votes.Vote"/> related events.
    /// </summary>
    public interface IVoteEvent
    {
        /// <summary>
        /// Gets the <see cref="Features.Votes.Vote"/> related to the event.
        /// </summary>
        public Vote Vote { get; }
    }
}
