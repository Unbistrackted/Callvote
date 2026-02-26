using Callvote.API.Enums;

namespace Callvote.API.Interfaces
{
    /// <summary>
    /// Represents the interface for providers.
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the <see cref="ProviderType"/> of this provider.
        /// </summary>
        public abstract ProviderType Type { get; }
    }
}
