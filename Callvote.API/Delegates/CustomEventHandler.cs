using System;

namespace Callvote.API.Delegates
{
    /// <summary>
    /// Represents the method that handles an event when event data of a specified type is provided.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event data passed to the event handler. Must derive from <see cref="EventArgs"/>.</typeparam>
    /// <param name="ev">The event data associated with the event being handled.</param>
    public delegate void CustomEventHandler<in TEventArgs>(TEventArgs ev)
        where TEventArgs : EventArgs;
}
