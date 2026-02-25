namespace Callvote.API.Votes.Enums
{
    /// <summary>
    /// Represents the enumeration for <see cref="Vote"/> types.
    /// </summary>
    public enum VoteType
    {
#pragma warning disable SA1602 // Enumeration items should be documented
        Binary,
        Ff,
        Kick,
        Kill,
        Nuke,
        RespawnWave,
        RestartRound,
        Custom,
#pragma warning restore SA1602 // Enumeration items should be documented
    }
}
