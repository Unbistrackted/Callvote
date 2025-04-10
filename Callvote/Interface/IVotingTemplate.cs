using Exiled.API.Features;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callvote.Interface
{
    public interface IVotingTemplate
    {
        Player CallVotePlayer { get; }
        string Question { get; }
        string VotingType { get; }
        CallvoteFunction Callback { get;}
        Dictionary<string, string> Options { get; }
    }
}
