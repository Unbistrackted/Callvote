using Callvote.Commands;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using RemoteAdmin;
using System.Collections.Generic;
using System.Timers;
using UnityEngine.Assertions.Must;

namespace Callvote.VoteHandlers
{
    public class Voting
    {
        public Player CallVotePlayer;
        public CallvoteFunction Callback;
        public Dictionary<string, int> Counter;
        public Dictionary<string, string> Options;
        public string Question;
        public Dictionary<string, string> PlayerVote;
        public CoroutineHandle VotingCoroutine;
        public string Response;

        public Voting(string question, Dictionary<string, string> options, Player player, CallvoteFunction callback)
        {
            CallVotePlayer = player;
            Question = question;
            Options = options;
            Callback = callback;
            PlayerVote = new Dictionary<string, string>();
            Counter = new Dictionary<string, int>();
            VotingCoroutine = new CoroutineHandle();
            foreach (string option in options.Keys) Counter[option] = 0;
            Response = Start();
        }

        public Voting(string question, Dictionary<string, string> options, Dictionary<string, string> votes,
            Dictionary<string, int> counter, CallvoteFunction callback)
        {
            Question = question;
            Options = options;
            PlayerVote = votes;
            Counter = counter;
            Callback = callback;
            VotingCoroutine = new CoroutineHandle();
            foreach (string option in options.Keys) Counter[option] = 0;
            Response = Start();
        }

        public string Start()
        {
            if (VotingAPI.CurrentVoting != null) { return $"<color=red>{Callvote.Instance.Translation.VotingInProgress}</color>"; }
            foreach (KeyValuePair<string, string> kvp in this.Options)
            {
                VoteCommand voteCommand = new VoteCommand(kvp.Key);
                QueryProcessor.DotCommandHandler.RegisterCommand(voteCommand);
            }
            if (!VotingAPI.CallvotePlayerDict.ContainsKey(CallVotePlayer))
            {
                VotingAPI.CallvotePlayerDict.Add(CallVotePlayer, 1);
            }
            VotingAPI.CallvotePlayerDict[CallVotePlayer]++;
            if (VotingAPI.CallvotePlayerDict[CallVotePlayer] > Callvote.Instance.Config.MaxAmountOfVotesPerRound && !CallVotePlayer.CheckPermission("cv.bypass")) { return Callvote.Instance.Translation.MaxVote; }
            VotingCoroutine = Timing.RunCoroutine(VotingAPI.StartVotingCoroutine(this));
            return Callvote.Instance.Translation.VotingStarted;
        }
        public string Stop()
        {
            if (VotingAPI.CurrentVoting == null) { return $"<color=red>{Callvote.Instance.Translation.NoVotingInProgress}</color>"; }
            Timing.KillCoroutines(VotingAPI.CurrentVoting.VotingCoroutine);
            foreach (KeyValuePair<string, string> kvp in VotingAPI.CurrentVoting.Options)
            {
                VoteCommand voteCommand = new VoteCommand(kvp.Key);
                QueryProcessor.DotCommandHandler.UnregisterCommand(voteCommand);
            }
            VotingAPI.CurrentVoting = null;
            return Callvote.Instance.Translation.VotingStoped;
        }

    }
}