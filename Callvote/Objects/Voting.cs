using Callvote.Commands;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using RemoteAdmin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.VoteHandlers
{
    public class Voting
    {
        public CallvoteFunction Callback { get; private set; }
        public string Question { get; private set; }
        public ConcurrentDictionary<string, int> Counter { get; private set; }
        public Dictionary<string, string> Options { get; private set; }
        public string VotingType { get; private set; }
        public Player CallVotePlayer { get; private set; }
        public long CallVoteId { get; private set; }
        public Dictionary<string, ICommand> CommandList { get; private set; }

        private Dictionary<string, string> PlayerVote;
        private CoroutineHandle VotingCoroutine;

        public Voting(string question, string votingType, Dictionary<string, string> options, Player player, CallvoteFunction callback)
        {
            CallVotePlayer = player;
            Question = question;
            Options = new Dictionary<string, string>(options);
            Callback = callback;
            PlayerVote = new Dictionary<string, string>();
            Counter = new ConcurrentDictionary<string, int>();
            VotingCoroutine = new CoroutineHandle();
            CommandList = new Dictionary<string, ICommand>();
            foreach (string option in options.Keys) Counter[option] = 0;
            VotingType = votingType;
            CallVoteId = DateTime.Now.ToBinary() + RandomNumber();
        }

        public Voting(string question, string votingType, Player player, CallvoteFunction callback)
        {
            CallVotePlayer = player;
            Question = question;
            Options = new Dictionary<string, string>(CallvoteAPI.Options);
            Callback = callback;
            PlayerVote = new Dictionary<string, string>();
            Counter = new ConcurrentDictionary<string, int>();
            VotingCoroutine = new CoroutineHandle();
            CommandList = new Dictionary<string, ICommand>();
            foreach (string option in Options.Keys) Counter[option] = 0;
            VotingType = votingType;
            CallVoteId = DateTime.Now.ToBinary() + RandomNumber();
        }

        public void Start()
        {
            if (!IsCallVotingAllowed()) { return; }
            RegisterVoteCommands();
            StartVotingCourotine();
            CallvoteAPI.Response = Callvote.Instance.Translation.VotingStarted;
            return;
        }
        public void Stop()
        {
            UnregisterVoteCommands();
            StopVotingCourotine();
            CallvoteAPI.Response = Callvote.Instance.Translation.VotingStoped;
            return;
        }

        public string Vote(Player player, string option)
        {
            string playerUserId = player.UserId;
            if (!Options.ContainsKey(option)) return Callvote.Instance.Translation.NoOptionAvailable.Replace("%Option%", option);

            if (PlayerVote.ContainsKey(playerUserId))
            {
                if (PlayerVote[playerUserId] == option) return Callvote.Instance.Translation.AlreadyVoted;
                Counter.AddOrUpdate(PlayerVote[playerUserId], 0, (key, value) => Math.Max(0, value - 1));
                PlayerVote[playerUserId] = option;
            }
            else
            {
                PlayerVote.Add(playerUserId, option);
            }

            Counter.AddOrUpdate(option, 1, (key, value) => value + 1);

            return Callvote.Instance.Translation.VoteAccepted.Replace("%Reason%", Options[option]);
        }

        public void Rig(string argument, int amount = 1)
        {
            if (!Counter.ContainsKey(argument))
            {
                CallvoteAPI.Response = Callvote.Instance.Translation.NoOptionAvailable.Replace("%Option%", argument);
                return;
            }
            Counter.AddOrUpdate(argument, amount, (key, value) => value + amount);
            CallvoteAPI.Response = $"Rigged {amount} votes for {argument}!";
            return;
        }

        private bool IsCallVotingAllowed()
        {
            if (CallvoteAPI.CurrentVoting != null && !Callvote.Instance.Config.EnableQueue)
            {
                CallvoteAPI.Response = Callvote.Instance.Translation.VotingInProgress;
                return false;
            }
            if (!CallvoteAPI.PlayerCallVotingAmount.ContainsKey(CallVotePlayer))
            {
                CallvoteAPI.PlayerCallVotingAmount.Add(CallVotePlayer, 1);
            }
            CallvoteAPI.PlayerCallVotingAmount[CallVotePlayer]++;
            if (CallvoteAPI.PlayerCallVotingAmount[CallVotePlayer] - 1 > Callvote.Instance.Config.MaxAmountOfVotesPerRound && !CallVotePlayer.CheckPermission("cv.bypass"))
            {
                CallvoteAPI.Response = Callvote.Instance.Translation.MaxVote;
                return false;
            }
            return true;
        }

        private void RegisterVoteCommands()
        {
            foreach (KeyValuePair<string, string> kvp in Options.ToList())
            {
                bool commandAlreadyRegistered = false;
                VoteCommand voteCommand = new VoteCommand(kvp.Key);
                if (QueryProcessor.DotCommandHandler.TryGetCommand(kvp.Key, out ICommand existingCommand))
                {
                    commandAlreadyRegistered = true;
                }
                if (commandAlreadyRegistered)
                {
                    Options.Remove(kvp.Key);
                    Options.Add("cv" + kvp.Key, kvp.Value);
                    Counter.TryRemove(kvp.Key, out int _int);
                    Counter.TryAdd("cv" + kvp.Key, 0);
                    voteCommand.Command = "cv" + kvp.Key;
                }
                CommandList.Add(kvp.Value, voteCommand);
                QueryProcessor.DotCommandHandler.RegisterCommand(voteCommand);
            }
        }

        private void UnregisterVoteCommands()
        {
            foreach (KeyValuePair<string, ICommand> command in CommandList)
            {
                QueryProcessor.DotCommandHandler.UnregisterCommand(command.Value);
            }
        }

        private void StopVotingCourotine()
        {
            Timing.KillCoroutines(VotingCoroutine);
        }

        private void StartVotingCourotine()
        {
            VotingCoroutine = Timing.RunCoroutine(CallvoteAPI.StartVotingCoroutine(this));
        }

        private long RandomNumber()
        {
            Random rng = new Random();
            return rng.Next(-1000, 1000);
        }
    }
}