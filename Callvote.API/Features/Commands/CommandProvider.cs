using Callvote.API.Delegates;
using Callvote.API.Enums;
using Callvote.API.Features.Votes;
using Callvote.API.Interfaces;

namespace Callvote.API.Features.Commands
{
    /// <summary>
    /// Represents the class that commands I/O functionality to Callvote.
    /// </summary>
    public abstract class CommandProvider : IProvider
    {
        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public ProviderType Type => ProviderType.Command;

        /// <summary>
        /// Registers the <see cref="VoteCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="VoteCommand"/> to register.</param>
        public abstract void RegisterCommand(VoteCommand command);

        /// <summary>
        /// Unregisters the <see cref="VoteCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="VoteCommand"/> to unregister.</param>
        public abstract void UnregisterCommand(VoteCommand command);

        /// <summary>
        /// Checks if the <see cref="VoteCommand"/> is registered.
        /// </summary>
        /// <param name="command">The <see cref="VoteCommand"/> to check.</param>
        /// <returns>True if the <see cref="VoteCommand"/> is registered, otherwise false.</returns>
        public abstract bool IsCommandRegistered(VoteCommand command);
    }
}
