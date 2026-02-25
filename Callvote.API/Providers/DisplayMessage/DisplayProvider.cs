using System;
using System.Collections.Generic;
using Callvote.API.Providers.Enums;
using Callvote.API.Providers.Interfaces;

namespace Callvote.API.Providers.DisplayMessage
{
    /// <summary>
    /// Represents the interface for the type that provides message displaying functionality to Callvote.
    /// </summary>
    public abstract class DisplayProvider : IProvider
    {
        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public ProviderType Type => ProviderType.DisplayMessage;

        /// <summary>
        /// Represents the interface for the type that provides message displaying functionality to Callvote.
        /// </summary>
        /// <param name="duration">The duration of the message.</param>
        /// <param name="content">The message that is going to be displayed.</param>
        /// <param name="players">The players to be displayed to.</param>
        public abstract void Show(TimeSpan duration, string content, IEnumerable<ReferenceHub> players);
    }
}
