using System.Collections.Generic;
using Callvote.API.Providers.DisplayMessage;
using Callvote.API.Providers.Enums;
using Callvote.API.Providers.Interfaces;

namespace Callvote.API.Providers.SendDiscordEmbed
{
    /// <summary>
    /// Represents the type that manages the <see cref="EmbedProvider"/> provider.
    /// </summary>
    public class DiscordEmbedHandler : ProviderHandler<EmbedProvider>
    {
        private EmbedProvider currentProvider;

        /// <summary>
        /// Gets the current instance of the DiscordEmbed Handlery system.
        /// </summary>
        public static DisplayHandler Instance { get; private set; } = new DisplayHandler();

        /// <inheritdoc/>
        public override Dictionary<string, EmbedProvider> Providers { get; } = [];

        /// <inheritdoc/>
        public override EmbedProvider CurrentProvider
        {
            get => this.currentProvider;
            internal set => this.currentProvider = value;
        }

        /// <inheritdoc/>
        public override ProviderType ProviderHandlerType => ProviderType.DisplayMessage;
    }
}
