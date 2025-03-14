using Callvote.Commands;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using RemoteAdmin;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Input;
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
        public List<CommandSystem.ICommand> CommandList;
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
            CommandList = new List<CommandSystem.ICommand>();
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
            CommandList = new List<CommandSystem.ICommand>();
            foreach (string option in options.Keys) Counter[option] = 0;
            Response = Start();
        }

        public string Start()
        {
            if (VotingAPI.CurrentVoting != null) { return Callvote.Instance.Translation.VotingInProgress; }
            if (!VotingAPI.CallvotePlayerDict.ContainsKey(CallVotePlayer))
            {
                VotingAPI.CallvotePlayerDict.Add(CallVotePlayer, 1);
            }
            VotingAPI.CallvotePlayerDict[CallVotePlayer]++;
            if (VotingAPI.CallvotePlayerDict[CallVotePlayer] - 1 > Callvote.Instance.Config.MaxAmountOfVotesPerRound && !CallVotePlayer.CheckPermission("cv.bypass")) { return Callvote.Instance.Translation.MaxVote; }
            foreach (KeyValuePair<string, string> kvp in this.Options.ToList())
            {
                VoteCommand voteCommand = new VoteCommand(kvp.Key);
                if (QueryProcessor.DotCommandHandler.TryGetCommand(kvp.Key, out CommandSystem.ICommand existingCommand))
                {
                    if (!this.Options.TryGetValue(kvp.Key, out string value))
                    {
                        return "There was a error handling your request.";
                    }
                    this.Options.Remove(kvp.Key);
                    this.Options.Add("cv" + kvp.Key, value);
                    this.Counter.Remove(kvp.Key);
                    this.Counter.Add("cv" + kvp.Key, 0);
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
            foreach (CommandSystem.ICommand command in CommandList)
            {
                QueryProcessor.DotCommandHandler.UnregisterCommand(command);
            }
            VotingAPI.Options.Clear();
            VotingAPI.CurrentVoting = null;
            return Callvote.Instance.Translation.VotingStoped;
        }

    }
}