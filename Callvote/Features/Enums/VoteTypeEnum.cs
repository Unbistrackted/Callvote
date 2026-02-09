namespace Callvote.Features.Enums
{
    /// <summary>
    /// Represents the enumeration for <see cref="Vote"/> types.
    /// </summary>
    public enum VoteTypeEnum
    {
#pragma warning disable SA1602 // Enumeration items should be documented - The enum values are self-explanatory.
        Binary,
        Ff,
        Kick,
        Kill,
        Nuke,
        RespawnWave,
        RestartRound,
        Custom,
#pragma warning restore SA1602 // Enumeration items should be documented - The enum values are self-explanatory.
    }
}
