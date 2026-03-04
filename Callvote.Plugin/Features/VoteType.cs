using Callvote.API.Features.Votes;

namespace Callvote.Features
{
    /// <summary>
    /// Represents the enumeration for <see cref="Vote"/> types.
    /// </summary>
    public enum VoteType
    {
        /// <summary>A<see cref="Vote"/> that has 2 options.</summary>
        Binary,

        /// <summary>A<see cref="Vote"/> that enables or disables ff.</summary>
        Ff,

        /// <summary>A<see cref="Vote"/> that kicks a player.</summary>
        Kick,

        /// <summary>A<see cref="Vote"/> that kills a player.</summary>
        Kill,

        /// <summary>A<see cref="Vote"/> that nukes the facility.</summary>
        Nuke,

        /// <summary>A<see cref="Vote"/> that respawns a wave.</summary>
        RespawnWave,

        /// <summary>A<see cref="Vote"/> that restarts the round.</summary>
        RestartRound,

        /// <summary>A<see cref="Vote"/> that has custom ammount of votes.</summary>
        Custom,
    }
}
