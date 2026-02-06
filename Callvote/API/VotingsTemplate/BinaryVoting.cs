#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using Callvote.Features;

namespace Callvote.API.VotingsTemplate
{
    /// <summary>
    /// Represents the type for the Binary Voting, which only contains the Yes and No options from the Translation File.
    /// Initializes a new instance of the <see cref="BinaryVoting"/> class.
    /// </summary>
    /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
    /// <param name="question"><see cref="Voting.Question"/>.</param>
    /// <param name="votingType"><see cref="Voting.VotingType"/>.</param>
    /// <param name="callback"><see cref="Voting.Callback"/>.</param>
    /// <param name="players"><see cref="Voting.AllowedPlayers"/>.</param>
    public class BinaryVoting(Player player, string question, string votingType, Action<Voting> callback = null, IEnumerable<Player> players = null) : Voting(player, question, votingType, callback, AddOptions(), players)
    {
        private static Dictionary<string, string> AddOptions()
        {
            Dictionary<string, string> options = [];
            options.Add(CallvotePlugin.Instance.Translation.CommandYes, CallvotePlugin.Instance.Translation.OptionYes);
            options.Add(CallvotePlugin.Instance.Translation.CommandNo, CallvotePlugin.Instance.Translation.OptionNo);

            return options;
        }
    }
}
