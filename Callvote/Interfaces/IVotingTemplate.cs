using Exiled.API.Features;
using System.Collections.Generic;

namespace Callvote.Interfaces
{
    public interface IVotingTemplate
    {
        Player CallVotePlayer { get; }
        string Question { get; }
        string VotingType { get; }
        CallvoteFunction Callback { get; }
        Dictionary<string, string> Options { get; }
    }
}
