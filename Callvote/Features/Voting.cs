#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.API;
using Callvote.Commands.VotingCommands;
using CommandSystem;
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
        public Player CallVotePlayer { get; init; }
        public string Question { get; init; }
        public string VotingType { get; init; }
        public Action<Voting> Callback { get; init; }
        public HashSet<Player> AllowedPlayers { get; init; }
        public Dictionary<string, string> Options { get; init; }
        public long CallVoteId { get; private set; }
        public ConcurrentDictionary<string, int> Counter { get; }
        public Dictionary<string, ICommand> CommandList { get; }

        private readonly Dictionary<string, string> _playerVote;
        private CoroutineHandle _votingCoroutine;

        public Voting(Player player, string question, string votingType, Action<Voting> callback, Dictionary<string, string> options = null, IEnumerable<Player> players = null)
        {
            CallVotePlayer = player;
            Question = question;
            Callback = callback;
            Options = new Dictionary<string, string>(options ?? VotingHandler.Options);
#if EXILED
            AllowedPlayers = [.. players ?? Player.List.Where(p => p.ReferenceHub.nicknameSync.NickSet && p.ReferenceHub.authManager.InstanceMode != 0)];
#else
            AllowedPlayers = [.. players ?? Player.ReadyList];
#endif
            _playerVote = [];
            CommandList = [];
            Counter = new ConcurrentDictionary<string, int>();
            _votingCoroutine = new CoroutineHandle();

            foreach (string option in Options.Keys)
                Counter[option] = 0;

            VotingType = votingType;
            CallVoteId = DateTime.Now.ToBinary() + RandomNumber();
        }

        internal void Start()
        {
            if (!VotingHandler.IsCallVotingAllowed(CallVotePlayer))
                return;

            RegisterVoteCommands();
            StartVotingCoroutine();

            VotingHandler.Response = Callvote.Instance.Translation.VotingStarted;
        }

        internal void Stop()
        {
            UnregisterVoteCommands();
            StopVotingCoroutine();

            VotingHandler.Response = Callvote.Instance.Translation.VotingStoped;
        }

        public string Vote(Player player, string option)
        {
            if (!AllowedPlayers.Contains(player))
                return Callvote.Instance.Translation.NoPermission;

            string playerUserId = player.UserId;

            if (!Options.ContainsKey(option))
                return Callvote.Instance.Translation.NoOptionAvailable.Replace("%Option%", option);

            if (_playerVote.ContainsKey(playerUserId))
            {
                if (_playerVote[playerUserId] == option)
                    return Callvote.Instance.Translation.AlreadyVoted;

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

        /// <summary>
        /// Coroutine that manages the <see cref="CurrentVoting"/> runtime. This method yields to MEC timing and repeatedly refreshes the voting display
        /// until the <see cref="CurrentVoting"/> duration has passed. When the courotine expires when the <see cref="CurrentVoting"/> is finished.
        /// </summary>
        /// <param name="newVote">The vote instance to run the coroutine for.</param>
        private IEnumerator<float> VotingCoroutine()
        {
            VotingHandler.CurrentVoting = this;
            int timerCounter = 0;
            DisplayMessageHelper.DisplayFirstMessage(Question, out string firstMessage);
            yield return Timing.WaitForSeconds(5f);

            while (true)
            {
                if (timerCounter >= Callvote.Instance.Config.VoteDuration + 1)
                {
                    VotingHandler.FinishVoting();
                    yield break;
                }

                DisplayMessageHelper.DisplayWhileVotingMessage(firstMessage);
                timerCounter++;
                yield return Timing.WaitForSeconds(Callvote.Instance.Config.RefreshInterval);
            }
        }

        private void RegisterVoteCommands()
        {
            bool alreadyRegistered = false;

            foreach (KeyValuePair<string, string> kvp in Options)
                if (QueryProcessor.DotCommandHandler.TryGetCommand(kvp.Key, out ICommand existingCommand))
                    alreadyRegistered = true;

            foreach (KeyValuePair<string, string> kvp in Options)
            {
                VoteCommand voteCommand = new(kvp.Key);

                if (alreadyRegistered)
                {
                    Options.Remove(kvp.Key);
                    Options.Add("cv" + kvp.Key, kvp.Value);
                    Counter.TryRemove(kvp.Key, out _);
                    Counter.TryAdd("cv" + kvp.Key, 0);
                    voteCommand.Command = "cv" + kvp.Key;
                }

                CommandList.Add(kvp.Key, voteCommand);
                QueryProcessor.DotCommandHandler.RegisterCommand(voteCommand);
            }
        }

        private void UnregisterVoteCommands()
        {
            foreach (KeyValuePair<string, ICommand> command in CommandList)
                QueryProcessor.DotCommandHandler.UnregisterCommand(command.Value);
        }

        private void StartVotingCoroutine()
        {
            _votingCoroutine = Timing.RunCoroutine(VotingCoroutine());
        }

        private void StopVotingCoroutine()
        {
            Timing.KillCoroutines(_votingCoroutine);
        }

        private long RandomNumber()
        {
            Random rng = new();
            return rng.Next(-1000, 1000);
        }
    }
}