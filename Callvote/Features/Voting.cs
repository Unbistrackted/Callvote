#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Callvote.API;
using Callvote.Configuration;
using MEC;

namespace Callvote.Features
{
    /// <summary>
    /// Represents the type that manages and creates the voting.
    /// </summary>
    public class Voting
    {
        private CoroutineHandle votingCoroutine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Voting"/> class.
        /// </summary>
#pragma warning disable SA1611 // Not Public
        internal Voting(Player player, string question, string votingType, Action<Voting> callback, HashSet<Vote> voteOptions = null, IEnumerable<Player> players = null)
#pragma warning restore SA1611
        {
            this.CallVotePlayer = player;
            this.Question = question;
            this.Callback = callback;
            this.VoteOptions = [.. voteOptions ?? VotingHandler.TemporaryVoteOptions];

#if EXILED
            this.AllowedPlayers = [.. players ?? Player.List.Where(p => p.ReferenceHub.nicknameSync.NickSet && p.ReferenceHub.authManager.InstanceMode != 0)];
#else
            this.AllowedPlayers = [.. players ?? Player.ReadyList];
#endif
            this.PlayerVote = [];
            this.Counter = new ConcurrentDictionary<Vote, int>();
            this.votingCoroutine = default;

            foreach (Vote vote in this.VoteOptions)
            {
                this.Counter[vote] = 0;
            }

            this.VotingType = votingType;
            this.CallVoteId = DateTime.Now.ToBinary() + this.RandomNumber();
        }

        /// <summary>
        /// Gets the player who called the <see cref="Voting"/> .
        /// </summary>
        public Player CallVotePlayer { get; init; }

        /// <summary>
        /// Gets the <see cref="Voting"/> question.
        /// </summary>
        public string Question { get; init; }

        /// <summary>
        /// Gets the <see cref="Voting"/> type.
        /// </summary>
        public string VotingType { get; init; }

        /// <summary>
        /// Gets the <see cref="Voting"/> callback.
        /// </summary>
        public Action<Voting> Callback { get; init; }

        /// <summary>
        /// Gets the allowed players that can see and vote on the <see cref="Voting"/> .
        /// </summary>
        public HashSet<Player> AllowedPlayers { get; init; }

        /// <summary>
        /// Gets the <see cref="HashSet{Vote}"/> with the available <see cref="Vote"/>s in the <see cref="Voting"/> .
        /// Key: Command/Option name. Value: Option/Label name for the command.
        /// </summary>
        public HashSet<Vote> VoteOptions { get; init; }

        /// <summary>
        /// Gets a value indicating whether it should only show the configured question and the counter.
        /// </summary>
        public bool ShouldOnlyShowQuestionAndCounter { get; init; } = true;

        /// <summary>
        /// Gets a value indicating whether it can show messages.
        /// </summary>
        public bool CanShowMessages { get; init; } = true;

        /// <summary>
        /// Gets the <see cref="Voting"/> ID.
        /// </summary>
        public long CallVoteId { get; private set; }

        /// <summary>
        /// Gets the Dictionary of Players with their <see cref="Vote"/> in the <see cref="Voting"/> .
        /// Key: Player. Value: <see cref="Vote"/>.
        /// </summary>
        public Dictionary<Player, Vote> PlayerVote { get; private set; }

        /// <summary>
        /// Gets the ammount of votes in a <see cref="Voting"/>.
        /// Key: Command/Option name. Value: Ammount of votes.
        /// </summary>
        public ConcurrentDictionary<Vote, int> Counter { get; }

        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        /// <summary>
        /// Makes a <see cref="Player"/> vote on a <see cref="Vote"/> of a <see cref="Voting"/>.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> who is will be voting.</param>
        /// <param name="vote">The <see cref="Vote"/> that will be selected.</param>
        /// <returns>A <see cref="string"/> representing if the vote process was sucessful or not.</returns>
        /// <remarks>
        /// The vote will only go through if the <see cref="votingCoroutine"/> is active.
        /// </remarks>
        public string VoteOption(Player player, Vote vote)
        {
            if (!votingCoroutine.IsRunning)
            {
                return Translation.NoVotingInProgress;
            }

            if (!this.AllowedPlayers.Contains(player))
            {
                return Translation.NoPermission;
            }

            if (!IsVotePresent(vote))
            {
                return Translation.NoOptionAvailable.Replace("%Option%", vote.Option ?? string.Empty);
            }

            if (this.PlayerVote.ContainsKey(player))
            {
                if (this.PlayerVote[player] == vote)
                {
                    return Translation.AlreadyVoted;
                }

                this.Counter.AddOrUpdate(this.PlayerVote[player], 0, (key, value) => Math.Max(0, value - 1)); // Removes the Value of the previous vote of the player

                this.PlayerVote[player] = vote;
            }
            else
            {
                this.PlayerVote.Add(player, vote);
            }

            this.Counter.AddOrUpdate(vote, 1, (key, value) => value + 1);

            return Translation.VoteAccepted.Replace("%Option%", vote.Option);
        }

        /// <summary>
        /// Gets the <see cref="Vote"/> in a <see cref="Voting"/> .
        /// </summary>
        /// <param name="command">The command that will be searched for.</param>
        /// <returns>A <see cref="Vote"/> if found, otherwise null.</returns>
        public Vote GetVote(string command)
        {
            return this.VoteOptions.Where(vote => vote.Command.Command == command && vote.IsCommandRegistered).FirstOrDefault();
        }

        /// <summary>
        /// Gets the <see cref="Vote"/> in a <see cref="Voting"/> .
        /// </summary>
        /// <param name="command">The string option that will be searched for.</param>
        /// <param name="vote">Returns a <see cref="Vote"/> if found, otherwise null.</param>
        /// <returns>If the specific <see cref="Vote"/> was found.</returns>
        public bool TryGetVote(string command, out Vote vote)
        {
            vote = GetVote(command);

            if (!IsVotePresent(vote))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a <see cref="Vote"/> exists in a <see cref="Voting"/> .
        /// </summary>
        /// <param name="vote">The <see cref="Vote"/> that will be searched for.</param>
        /// <returns>A true if found, otherwise false.</returns>
        public bool IsVotePresent(Vote vote)
        {
            if (vote == null && vote.IsCommandRegistered)
            {
                return false;
            }

            return this.VoteOptions.Contains(vote);
        }

        /// <summary>
        /// Gets a <see cref="Vote"/> percentage based on <see cref="AllowedPlayers"/> in a <see cref="Voting"/> .
        /// </summary>
        /// <param name="vote">The <see cref="Vote"/> that will be searched for.</param>
        /// <returns>A int value as a percentage.</returns>
        public int GetVotePercentage(Vote vote)
        {
            if (!IsVotePresent(vote))
            {
                return 0;
            }

            return (int)(Counter[vote] / (float)AllowedPlayers.Count * 100f);
        }

        /// <summary>
        /// Rigs the <see cref="Voting"/> <see cref="Counter"/> .
        /// </summary>
        /// <param name="option">The option that will be rigged.</param>
        /// <param name="amount">The ammount of votes added to that option.</param>
        /// <returns>A <see cref="string"/> representing if the vote rigging was sucessful or not.</returns>
        /// <remarks>
        /// The vote will only go through if the <see cref="votingCoroutine"/> is active.
        /// </remarks>
        public string Rig(string option, int amount = 1)
        {
            Vote vote = this.GetVote(option);

            if (!IsVotePresent(vote))
            {
                return Translation.NoOptionAvailable.Replace("%Option%", option);
            }

            this.Counter.AddOrUpdate(vote, amount, (key, value) => value + amount);

            return $"Rigged {amount} votes for {option}!";
        }

        /// <summary>
        /// Starts the <see cref="Voting"/> by registering the commands and starting the coroutine.
        /// </summary>
        /// <returns>The response message set by operations on the handler (e.g. "Queue is full" or "Voting enqueued").</returns>
        internal string Start()
        {
            if (!VotingHandler.IsCallVotingAllowed(this.CallVotePlayer))
            {
                return Translation.MaxVote;
            }

            this.RegisterVoteCommands();
            this.StartVotingCoroutine();

            return Translation.VotingStarted;
        }

        /// <summary>
        /// Stops the <see cref="Voting"/> by unregistering the command and stopping the coroutine.
        /// </summary>
        internal void Stop()
        {
            this.UnregisterVoteCommands();
            this.StopVotingCoroutine();
        }

        private IEnumerator<float> VotingCoroutine()
        {
            int timerCounter = 0;
            DisplayMessageHelper.DisplayFirstMessage(this.Question, out string firstMessage);
            yield return Timing.WaitForSeconds(5f);

            while (true)
            {
                if (timerCounter >= Config.VoteDuration + 1)
                {
                    VotingHandler.FinishVoting();
                    yield break;
                }

                DisplayMessageHelper.DisplayWhileVotingMessage(firstMessage);
                timerCounter++;
                yield return Timing.WaitForSeconds(Config.RefreshInterval);
            }
        }

        private void RegisterVoteCommands()
        {
            foreach (Vote vote in this.VoteOptions)
            {
                vote.RegisterCommand();
            }
        }

        private void UnregisterVoteCommands()
        {
            foreach (Vote vote in this.VoteOptions)
            {
                vote.UnregisterCommand();
            }
        }

        private void StartVotingCoroutine()
        {
            this.votingCoroutine = Timing.RunCoroutine(this.VotingCoroutine());
        }

        private void StopVotingCoroutine()
        {
            Timing.KillCoroutines(this.votingCoroutine);
        }

        private long RandomNumber()
        {
            Random rng = new();
            return rng.Next(-1000, 1000);
        }
    }
}