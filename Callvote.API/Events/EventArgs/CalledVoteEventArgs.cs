using Callvote.API.Events.Interfaces;
using Callvote.API.Features.Votes;

namespace Callvote.API.Events.EventArgs
{
    /// <summary>
    /// Contains all information about a <see cref="Vote"/> that has been called.
    /// </summary>
    public class CalledVoteEventArgs : System.EventArgs, IReferenceHubEvent, IVoteEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalledVoteEventArgs"/> class.
        /// </summary>
        /// <param name="vote"><inheritdoc cref="Vote"/></param>
        public CalledVoteEventArgs(Vote vote)
        {
            this.Vote = vote;
        }

        /// <inheritdoc />
        public ReferenceHub ReferenceHub => this.Vote.CallVotePlayer;

        /// <inheritdoc />
        public Vote Vote { get; }
    }
}
