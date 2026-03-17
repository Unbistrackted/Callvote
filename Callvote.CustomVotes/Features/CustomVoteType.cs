using Callvote.API.Features.Votes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callvote.CustomVotes.Features
{
    /// <summary>
    /// Represents the enumeration for custom <see cref="Vote"/> types.
    /// </summary>
    public enum CustomVoteType
    {
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
    }
}
