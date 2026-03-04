using Callvote.API.Interfaces;

namespace Callvote.API.Enums
{
    /// <summary>
    /// Represents the enumeration for the <see cref="IProvider"/> type.
    /// </summary>
    public enum ProviderType
    {
        /// <summary>A<see cref="IProvider"/> that handles the displaying of messages.</summary>
        DisplayMessage,

        /// <summary>A<see cref="IProvider"/> that handles the commands.</summary>
        Command,
    }
}
