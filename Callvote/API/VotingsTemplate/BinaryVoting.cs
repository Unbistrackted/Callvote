#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.Features;
using System.Collections.Generic;
using System;

namespace Callvote.API.VotingsTemplate
{
    public class BinaryVoting(Player player, string question, string votingType, Action<Voting> callback = null, IEnumerable<Player> players = null) : Voting(player, question, votingType, callback, AddOptions(), players)
    {
        public static Dictionary<string, string> AddOptions()
        {
            Dictionary<string, string> options = [];
            options.Add(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            options.Add(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);

            return options;
        }
    }
}
