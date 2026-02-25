using Callvote.API.Providers.Interfaces;

namespace Callvote.API.Providers.Enums
{
    /// <summary>
    /// Represents the enumeration for the <see cref="IProvider"/> type.
    /// </summary>
    public enum ProviderType
    {
#pragma warning disable SA1602 // Enumeration items should be documented
        DisplayMessage,
        DiscordEmbed,
    }
}
