#if EXILED
using Exiled.API.Features;
#else
using Callvote.Features;
using LabApi.Features.Wrappers;
using System;

#endif
using System.Collections.Generic;

namespace Callvote.Features.Interfaces
{
    public interface IVotingTemplate
    {
        Player CallVotePlayer { get; }
        string Question { get; }
        string VotingType { get; }
        Action<Voting> Callback { get; }
        Dictionary<string, string> Options { get; }
    }
}
