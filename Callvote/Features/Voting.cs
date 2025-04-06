using Callvote.API;
using Callvote.Commands.VotingCommands;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using RemoteAdmin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.Features
{
    public class Voting
    {
        public CallvoteFunction Callback { get; private set; }
        public string Question { get; private set; }
        public string VotingType { get; private set; }
        public long CallVoteId { get; private set; }
        public ConcurrentDictionary<string, int> Counter { get; }
        public Dictionary<string, string> Options { get; }
        public Dictionary<string, ICommand> CommandList { get; }
        public Player CallVotePlayer { get; }

        private Dictionary<string, string> _playerVote;
        private CoroutineHandle _votingCoroutine;

        public Voting(string question, string votingType, Player player, CallvoteFunction callback, Dictionary<string, string> options = null)
        {
            CallVotePlayer = player;
            Question = question;
            Options = new Dictionary<string, string>(options ?? VotingHandler.Options);
            Callback = callback;
            _playerVote = new Dictionary<string, string>();
            Counter = new ConcurrentDictionary<string, int>();
            _votingCoroutine = new CoroutineHandle();
            CommandList = new Dictionary<string, ICommand>();
            foreach (string option in Options.Keys) Counter[option] = 0;
            VotingType = votingType;
            CallVoteId = DateTime.Now.ToBinary() + RandomNumber();
        }

        public void Start()
        {
            if (!IsCallVotingAllowed()) { return; }
            RegisterVoteCommands();
            StartVotingCoroutine();
            VotingHandler.Response = Callvote.Instance.Translation.VotingStarted;
        }
        public void Stop()
        {
            UnregisterVoteCommands();
            StopVotingCoroutine();
            VotingHandler.Response = Callvote.Instance.Translation.VotingStoped;
        }

        public string Vote(Player player, string option)
        {
            string playerUserId = player.UserId;
            if (!Options.ContainsKey(option)) return Callvote.Instance.Translation.NoOptionAvailable.Replace("%Option%", option);

            if (_playerVote.ContainsKey(playerUserId))
            {
                if (_playerVote[playerUserId] == option) return Callvote.Instance.Translation.AlreadyVoted;
                Counter.AddOrUpdate(_playerVote[playerUserId], 0, (key, value) => Math.Max(0, value - 1));
                _playerVote[playerUserId] = option;
            }
            else
            {
                _playerVote.Add(playerUserId, option);
            }

            Counter.AddOrUpdate(option, 1, (key, value) => value + 1);

            return Callvote.Instance.Translation.VoteAccepted.Replace("%Option%", Options[option]);
        }

        public void Rig(string argument, int amount = 1)
        {
            if (!Counter.ContainsKey(argument))
            {
                VotingHandler.Response = Callvote.Instance.Translation.NoOptionAvailable.Replace("%Option%", argument);
                return;
            }
            Counter.AddOrUpdate(argument, amount, (key, value) => value + amount);
            VotingHandler.Response = $"Rigged {amount} votes for {argument}!";
        }

        private bool IsCallVotingAllowed()
        {
            if (VotingHandler.CurrentVoting != null && !Callvote.Instance.Config.EnableQueue)
            {
                VotingHandler.Response = Callvote.Instance.Translation.VotingInProgress;
                return false;
            }
            if (!VotingHandler.PlayerCallVotingAmount.ContainsKey(CallVotePlayer))
            {
                VotingHandler.PlayerCallVotingAmount.Add(CallVotePlayer, 1);
            }
            VotingHandler.PlayerCallVotingAmount[CallVotePlayer]++;
            if (VotingHandler.PlayerCallVotingAmount[CallVotePlayer] - 1 > Callvote.Instance.Config.MaxAmountOfVotesPerRound && !CallVotePlayer.CheckPermission("cv.bypass"))
            {
                VotingHandler.Response = Callvote.Instance.Translation.MaxVote;
                return false;
            }
            return true;
        }

        private void RegisterVoteCommands()
        {
            bool alreadyRegistered = false;
            foreach (KeyValuePair<string, string> kvp in Options.ToList())
            {
                if (QueryProcessor.DotCommandHandler.TryGetCommand(kvp.Key, out ICommand existingCommand))
                {
                    alreadyRegistered = true;
                }
            }
            foreach (KeyValuePair<string, string> kvp in Options.ToList())
            {
                VoteCommand voteCommand = new VoteCommand(kvp.Key);
                if (alreadyRegistered)
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

        private void StartVotingCoroutine()
        {
            _votingCoroutine = Timing.RunCoroutine(VotingHandler.VotingCoroutine(this));
        }

        private void StopVotingCoroutine()
        {
            Timing.KillCoroutines(_votingCoroutine);
        }

        private long RandomNumber()
        {
            Random rng = new Random();
            return rng.Next(-1000, 1000);
        }
    }
}