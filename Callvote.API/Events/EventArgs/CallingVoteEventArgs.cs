using Callvote.API.Events.Interfaces;
using Callvote.API.Features.Votes;

namespace Callvote.API.Events.EventArgs
{
    /// <summary>
    /// Contains all information about a <see cref="Vote"/> that is being called.
    /// </summary>
    public class CallingVoteEventArgs : System.EventArgs, IReferenceHubEvent, IVoteEvent, IDeniableEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallingVoteEventArgs"/> class.
        /// </summary>
        /// <param name="referenceHub"><inheritdoc cref="ReferenceHub"/></param>
        /// <param name="vote"><inheritdoc cref="Vote"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public CallingVoteEventArgs(ReferenceHub referenceHub, Vote vote, bool isAllowed = true)
        {
            this.ReferenceHub = referenceHub;
            this.Vote = vote;
            this.IsAllowed = isAllowed;
        }

        /// <inheritdoc />
        public ReferenceHub ReferenceHub => this.Vote.CallVotePlayer;

        /// <inheritdoc />
        public Vote Vote { get; }

        /// <inheritdoc />
        public bool IsAllowed { get; set; }
    }
}
