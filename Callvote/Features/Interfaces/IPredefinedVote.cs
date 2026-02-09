#if EXILED
#else
using Callvote.Features;
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;

namespace Callvote.Features.Interfaces
{
    /// <summary>
    /// Represents the interface for Vote templates.
    /// </summary>
    public interface IPredefinedVote
    {
        /// <summary>
        /// Gets <see cref="Vote.Callback"/> of the template.
        /// </summary>
        Action<Vote> Callback { get; }

        /// <summary>
        /// Gets <see cref="Vote.VoteOptions"/> of the template.
        /// </summary>
        HashSet<VoteOption> VoteOptions { get; }
    }
}
