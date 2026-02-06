namespace Callvote.Features.Enums
{
    /// <summary>
    /// Represents the enumeration for votings type.
    /// </summary>
    internal enum VotingTypeEnum
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
