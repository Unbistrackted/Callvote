using Callvote.Commands;
using Exiled.API.Features;
using MEC;
using RemoteAdmin;
using System.Collections.Generic;
using System.Timers;
using UnityEngine.Assertions.Must;

namespace Callvote.VoteHandlers
{
    public class Voting
    {
        public CallvoteFunction Callback;
        public Dictionary<string, int> Counter; 
        public Dictionary<string, string> Options;
        public string Question;
        public Dictionary<string, string> PlayerVote;
        public CoroutineHandle VotingCoroutine;
        public string Response;

        public Voting(string question, Dictionary<string, string> options, CallvoteFunction callback)
        {
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
            if (VoteAPI.CurrentVoting != null) { return $"<color=red>{Plugin.Instance.Translation.VotingInProgress}</color>"; }
            VotingCoroutine = Timing.RunCoroutine(VoteAPI.StartVoteCoroutine(this));
            foreach (KeyValuePair<string, string> kvp in this.Options)
            {
                VoteCommand voteCommand = new VoteCommand(kvp.Key);
                QueryProcessor.DotCommandHandler.RegisterCommand(voteCommand);
            }
            return Plugin.Instance.Translation.VotingStarted;
        }
        public string Stop()
        {
            if (VoteAPI.CurrentVoting == null) { return $"<color=red>{Plugin.Instance.Translation.NoVotingInProgress}</color>"; }
            Timing.KillCoroutines(VoteAPI.CurrentVoting.VotingCoroutine);
            foreach (KeyValuePair<string, string> kvp in VoteAPI.CurrentVoting.Options)
            {
                VoteCommand voteCommand = new VoteCommand(kvp.Key);
                QueryProcessor.DotCommandHandler.UnregisterCommand(voteCommand);
            }
            VoteAPI.CurrentVoting = null;
            return Plugin.Instance.Translation.VotingStoped;
        }

    }
}