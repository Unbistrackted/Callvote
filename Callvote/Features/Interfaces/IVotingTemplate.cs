#if EXILED
using Exiled.API.Features;
#else
using Callvote.Features;
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;

namespace Callvote.Features.Interfaces
{
    /// <summary>
    /// Represents the interface for voting templates.
    /// </summary>
    public interface IVotingTemplate
    {
        /// <summary>
        /// Gets <see cref="Voting.Callback"/> of the template.
        /// </summary>
        Action<Voting> Callback { get; }

        /// <summary>
        /// Gets <see cref="Voting.Options"/> of the template.
        /// </summary>
        Dictionary<string, string> Options { get; }
    }
}
