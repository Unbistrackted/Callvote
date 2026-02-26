using Callvote.API.Features.Votes;

namespace Callvote.API.Enums
{
    /// <summary>
    /// Represents the enumeration for <see cref="Vote"/> types.
    /// </summary>
    public enum VoteType
    {
#pragma warning disable SA1602
        Binary,
        Ff,
        Kick,
        Kill,
        Nuke,
        RespawnWave,
        RestartRound,
        Custom,
    }
}
