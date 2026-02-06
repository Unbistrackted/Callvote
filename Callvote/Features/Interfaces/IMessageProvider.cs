#if EXILED
using Player = Exiled.API.Features.Player;
#else
using Player = LabApi.Features.Wrappers.Player;
#endif
using System;
using System.Collections.Generic;

namespace Callvote.Features.Interfaces
{
    /// <summary>
    /// Represents the interface for the type that provides message displaying functionality to Callvote.
    /// </summary>
    public interface IMessageProvider
    {
        /// <summary>
        /// Represents the interface for the type that provides message displaying functionality to Callvote.
        /// </summary>
        /// <param name="duration">The duration of the message.</param>
        /// <param name="content">The message that is going to be displayed.</param>
        /// <param name="players">The players to be displayed to.</param>
        void DisplayMessage(TimeSpan duration, string content, HashSet<Player> players);
    }
}
