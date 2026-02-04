#if EXILED
using Exiled.API.Features;
using System;

#else
using Callvote.Features;
using LabApi.Features.Wrappers;
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
