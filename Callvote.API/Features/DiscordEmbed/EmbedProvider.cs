using Callvote.API.Enums;
using Callvote.API.Features.Core.Interfaces;
using Callvote.API.Features.Votes;

namespace Callvote.API.Features.DiscordEmbed
{
    /// <summary>
    /// Represents the interface for Webhook Providers, which are used to send messages via Webhooks to Discord.
    /// </summary>
    public abstract class EmbedProvider : IProvider
    {
        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public ProviderType Type => ProviderType.DisplayMessage;

        /// <summary>
        /// Sends a Vote Results message using the <see cref="EmbedProvider"/>.
        /// </summary>
        /// <param name="vote">The <see cref="Vote"/> that just ended.</param>
        public abstract void SendVoteResults(Vote vote);

        /// <summary>
        /// Sends a Vote Started message using the <see cref="EmbedProvider"/>.
        /// </summary>
        /// <param name="vote">The <see cref="Vote"/> that just started.</param>
        public abstract void SendVoteStarted(Vote vote);
    }
}
