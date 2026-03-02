namespace Callvote.API.Events.Interfaces
{
    /// <summary>
    /// Event args used for deniable events.
    /// </summary>
    internal interface IDeniableEvent
    {
        /// <summary>
        /// Gets or sets a value indicating whether the event was allowed or not.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}
