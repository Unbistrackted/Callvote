using System.Collections.Generic;
using System.Linq;
using Callvote.Commands;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using RemoteAdmin;

namespace Callvote.VoteHandlers
{
    public class Voting
    {
        public CallvoteFunction Callback;
        public Player CallVotePlayer;
        public List<ICommand> CommandList;
        public Dictionary<string, int> Counter;
        public Dictionary<string, string> Options;
        public Dictionary<string, string> PlayerVote;
        public string Question;
        public string Response;
        public CoroutineHandle VotingCoroutine;

        public Voting(string question, Dictionary<string, string> options, Player player, CallvoteFunction callback)
        {
            CallVotePlayer = player;
            Question = question;
            Options = options;
            Callback = callback;
            PlayerVote = new Dictionary<string, string>();
            Counter = new Dictionary<string, int>();
            VotingCoroutine = new CoroutineHandle();
            CommandList = new List<ICommand>();
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
            CommandList = new List<ICommand>();
            foreach (string option in options.Keys) Counter[option] = 0;
            Response = Start();
        }

        public string Start()
        {
            if (VotingAPI.CurrentVoting != null) { return Callvote.Instance.Translation.VotingInProgress; }
            if (!VotingAPI.PlayerCallVotingAmount.ContainsKey(CallVotePlayer))
            {
                VotingAPI.PlayerCallVotingAmount.Add(CallVotePlayer, 1);
            }
            VotingAPI.PlayerCallVotingAmount[CallVotePlayer]++;
            if (VotingAPI.PlayerCallVotingAmount[CallVotePlayer] - 1 > Callvote.Instance.Config.MaxAmountOfVotesPerRound && !CallVotePlayer.CheckPermission("cv.bypass")) { return Callvote.Instance.Translation.MaxVote; }
            foreach (KeyValuePair<string, string> kvp in Options.ToList())
            {
                VoteCommand voteCommand = new VoteCommand(kvp.Key);
                if (QueryProcessor.DotCommandHandler.TryGetCommand(kvp.Key, out ICommand existingCommand))
                {
                    if (!Options.TryGetValue(kvp.Key, out string value))
                    {
                        return "There was a error handling your request.";
                    }
                    Options.Remove(kvp.Key);
                    Options.Add("cv" + kvp.Key, value);
                    Counter.Remove(kvp.Key);
                    Counter.Add("cv" + kvp.Key, 0);
                    voteCommand.Command = "cv" + kvp.Key;
                }
                CommandList.Add(voteCommand);
                QueryProcessor.DotCommandHandler.RegisterCommand(voteCommand);
            }
            VotingCoroutine = Timing.RunCoroutine(VotingAPI.StartVotingCoroutine(this));
            return Callvote.Instance.Translation.VotingStarted;
        }
        public string Stop()
        {
            if (VotingAPI.CurrentVoting == null) { return Callvote.Instance.Translation.NoVotingInProgress; }
            Timing.KillCoroutines(VotingAPI.CurrentVoting.VotingCoroutine);
            foreach (ICommand command in CommandList)
            {
                QueryProcessor.DotCommandHandler.UnregisterCommand(command);
            }
            VotingAPI.Options.Clear();
            VotingAPI.CurrentVoting = null;
            return Callvote.Instance.Translation.VotingStoped;
        }
    }
}