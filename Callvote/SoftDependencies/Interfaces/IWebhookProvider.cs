using Callvote.Features;

namespace Callvote.SoftDependencies.Interfaces
{
    /// <summary>
    /// Represents the interface for Webhook Providers, which are used to send messages via Webhooks to Discord.
    /// </summary>
    public interface IWebhookProvider
    {
        /// <summary>
        /// Sends a Vote Results message to the Webhook with the specified question, player and votes.
        /// </summary>
        /// <param name="vote">The <see cref="Vote"/> that just ended.</param>
        public abstract void SendVoteResults(Vote vote);

        // I will add more methods like "CalledVote/VoteStarted" in the future.
    }
}
