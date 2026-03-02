namespace Callvote.API.Events.Interfaces
{
    /// <summary>
    /// Event args used for all ReferenceHub related events.
    /// </summary>
    public interface IReferenceHubEvent
    {
        /// <summary>
        /// Gets the ReferenceHub triggering the event.
        /// </summary>
        public ReferenceHub ReferenceHub { get; }
    }
}
