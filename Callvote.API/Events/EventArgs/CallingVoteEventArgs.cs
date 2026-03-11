using Callvote.API.Events.Interfaces;
using Callvote.API.Features.Votes;

namespace Callvote.API.Events.EventArgs
{
    /// <summary>
    /// Contains all information about a <see cref="Vote"/> that is being called.
    /// </summary>
    public class CallingVoteEventArgs : System.EventArgs, IUserIndentifierEvent, IVoteEvent, IDeniableEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallingVoteEventArgs"/> class.
        /// </summary>
        /// <param name="vote"><inheritdoc cref="Vote"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public CallingVoteEventArgs(Vote vote, bool isAllowed = true)
        {
            this.Vote = vote;
            this.IsAllowed = isAllowed;
        }

        /// <inheritdoc />
        public UserIndentifier User => this.Vote.CallVotePlayer;

        /// <inheritdoc />
        public Vote Vote { get; }

        /// <inheritdoc />
        public bool IsAllowed { get; set; }
    }
}
