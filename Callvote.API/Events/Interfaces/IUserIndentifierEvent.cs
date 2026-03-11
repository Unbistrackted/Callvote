using Callvote.API.Features.Votes;

namespace Callvote.API.Events.Interfaces
{
    /// <summary>
    /// Event args used for all UserIndentifier related events.
    /// </summary>
    public interface IUserIndentifierEvent
    {
        /// <summary>
        /// Gets the UserIndentifier triggering the event.
        /// </summary>
        public UserIndentifier User { get; }
    }
}
