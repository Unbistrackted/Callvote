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
    /// Represents the type that manages and creates the <see cref="Vote"/>.
    /// Responsible for the <see cref="Vote"/> cycle, and managing the <see cref="Features.VoteOption"/>s.
    /// </summary>
    public class Vote
    {
        private CoroutineHandle voteCoroutine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vote"/> class.
        /// </summary>
#pragma warning disable SA1611 // Not Public
        internal Vote(Player player, string question, string voteType, Action<Vote> callback, HashSet<VoteOption> voteOptions = null, IEnumerable<Player> players = null)
#pragma warning restore SA1611
        {
            this.CallVotePlayer = player;
            this.Question = question;
            this.Callback = callback;
            this.VoteOptions = [.. voteOptions ?? VoteHandler.TemporaryVoteOptions];

#if EXILED
            this.AllowedPlayers = [.. players ?? Player.List.Where(p => p.ReferenceHub.nicknameSync.NickSet && p.ReferenceHub.authManager.InstanceMode != 0)];
#else
            this.AllowedPlayers = [.. players ?? Player.ReadyList];
#endif
            this.PlayerVote = [];
            this.Counter = new ConcurrentDictionary<VoteOption, int>();
            this.voteCoroutine = default;

            foreach (VoteOption vote in this.VoteOptions)
            {
                this.Counter[vote] = 0;
            }

            this.VoteType = voteType;
            this.VoteId = DateTime.Now.ToBinary() + this.RandomNumber();
        }

        /// <summary>
        /// Gets the player who called the <see cref="Vote"/> .
        /// </summary>
        public Player CallVotePlayer { get; init; }

        /// <summary>
        /// Gets the <see cref="Vote"/> question.
        /// </summary>
        public string Question { get; init; }

        /// <summary>
        /// Gets the <see cref="Vote"/> type.
        /// </summary>
        public string VoteType { get; init; }

        /// <summary>
        /// Gets the <see cref="Vote"/> callback.
        /// </summary>
        public Action<Vote> Callback { get; init; }

        /// <summary>
        /// Gets the allowed players that can see and vote on the <see cref="Vote"/> .
        /// </summary>
        public HashSet<Player> AllowedPlayers { get; init; }

        /// <summary>
        /// Gets the <see cref="HashSet{Vote}"/> with the available <see cref="Features.VoteOption"/>s in the <see cref="Vote"/> .
        /// </summary>
        /// <remarks><see cref="Features.VoteOption"/>s can have the same <see cref="VoteOption.Option"/>, but not the <see cref="VoteOption.Command"/>.</remarks>
        public HashSet<VoteOption> VoteOptions { get; init; }

        /// <summary>
        /// Gets a value indicating whether it should only show the configured question and the counter.
        /// </summary>
        public bool ShouldOnlyShowQuestionAndCounter { get; init; } = false;

        /// <summary>
        /// Gets a value indicating whether it can show messages.
        /// </summary>
        public bool CanShowMessages { get; init; } = true;

        /// <summary>
        /// Gets the <see cref="Vote"/> ID.
        /// </summary>
        public long VoteId { get; private set; }

        /// <summary>
        /// Gets the Dictionary of Players with their <see cref="Features.VoteOption"/> in the <see cref="Vote"/> .
        /// Key: Player. Value: <see cref="Features.VoteOption"/>.
        /// </summary>
        public Dictionary<Player, VoteOption> PlayerVote { get; private set; }

        /// <summary>
        /// Gets the ammount of votes of a <see cref="Features.VoteOption"/> in a <see cref="Vote"/>.
        /// Key: <see cref="Features.VoteOption"/>. Value: Ammount of votes.
        /// </summary>
        public ConcurrentDictionary<VoteOption, int> Counter { get; }

        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        /// <summary>
        /// Makes a <see cref="Player"/> vote on a <see cref="Features.VoteOption"/> of a <see cref="Vote"/>.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> who is will be submiting the vote option.</param>
        /// <param name="vote">The <see cref="Features.VoteOption"/> that will be selected.</param>
        /// <returns>A <see cref="bool"/> representing if the vote process was sucessful or not.</returns>
        /// <remarks>
        /// The vote will only go through if the <see cref="voteCoroutine"/> is active.
        /// </remarks>
        public bool SubmitVoteOption(Player player, VoteOption vote)
        {
            if (!this.voteCoroutine.IsRunning)
            {
                return false;
            }

            if (!this.AllowedPlayers.Contains(player))
            {
                return false;
            }

            if (!this.IsVoteOptionPresent(vote))
            {
                return false;
            }

            if (this.PlayerVote.ContainsKey(player))
            {
                if (this.PlayerVote[player] == vote)
                {
                    return false;
                }

                this.Counter.AddOrUpdate(this.PlayerVote[player], 0, (key, value) => Math.Max(0, value - 1)); // Removes the Value of the previous vote of the player

                this.PlayerVote[player] = vote;
            }
            else
            {
                this.PlayerVote.Add(player, vote);
            }

            this.Counter.AddOrUpdate(vote, 1, (key, value) => value + 1);

            return true;
        }

        /// <summary>
        /// Gets the <see cref="Features.VoteOption"/> in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="command">The command that will be searched for.</param>
        /// <returns>A <see cref="Features.VoteOption"/> if found, otherwise null.</returns>
        public VoteOption GetVoteOptionFromCommand(string command) => this.VoteOptions.FirstOrDefault(vote => vote.Command.Command == command);

        /// <summary>
        /// Gets the <see cref="Features.VoteOption"/> in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="option">The option that will be searched for.</param>
        /// <returns>A <see cref="HashSet{Vote}"/> .</returns>
        /// <remarks><see cref="Features.VoteOption"/>s in <see cref="VoteOptions"/> can have the same <see cref="VoteOption.Option"/>, consider using <see cref="GetVoteOptionFromCommand"/> if you made sure the command is not already registed by another plugin.</remarks>
        public HashSet<VoteOption> GetVoteOptions(string option) => [.. this.VoteOptions.Where(vote => vote.Option == option)];

        /// <summary>
        /// Gets the <see cref="Features.VoteOption"/> in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="command">The string option that will be searched for.</param>
        /// <param name="vote">Returns a <see cref="Features.VoteOption"/> if found, otherwise null.</param>
        /// <returns>If the specific <see cref="Features.VoteOption"/> was found.</returns>
        public bool TryGetVoteOptionFromCommand(string command, out VoteOption vote)
        {
            vote = this.GetVoteOptionFromCommand(command);

            if (!this.IsVoteOptionPresent(vote))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a <see cref="Features.VoteOption"/> exists in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="vote">The <see cref="Features.VoteOption"/> that will be searched for.</param>
        /// <returns>A true if found, otherwise false.</returns>
        public bool IsVoteOptionPresent(VoteOption vote)
        {
            if (vote == null)
            {
                return false;
            }

            return this.VoteOptions.Contains(vote);
        }

        /// <summary>
        /// Gets a <see cref="Features.VoteOption"/> percentage based on <see cref="AllowedPlayers"/> in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="vote">The <see cref="Features.VoteOption"/> that will be searched for.</param>
        /// <returns>A int value as a percentage.</returns>
        public int GetVoteOptionPercentage(VoteOption vote)
        {
            if (!this.IsVoteOptionPresent(vote))
            {
                return 0;
            }

            return (int)(this.Counter[vote] / (float)this.AllowedPlayers.Count * 100f);
        }

        /// <summary>
        /// Rigs the <see cref="Vote"/> <see cref="Counter"/> .
        /// </summary>
        /// <param name="option">The option that will be rigged.</param>
        /// <param name="vote">The <see cref="Features.VoteOption"/> from the <see cref="Vote"/>.</param>
        /// <param name="amount">The ammount of votes added to that option.</param>
        /// <returns>If the vote rigging was sucessful or not.</returns>
        /// <remarks>
        /// The vote will only go through if the <see cref="voteCoroutine"/> is active.
        /// </remarks>
        public bool Rig(string option, out VoteOption vote, int amount = 1)
        {
            vote = this.GetVoteOptionFromCommand(option);

            if (!this.IsVoteOptionPresent(vote))
            {
                return false;
            }

            this.Counter.AddOrUpdate(vote, amount, (key, value) => value + amount);

            return true;
        }

        /// <summary>
        /// Starts the <see cref="Vote"/> by registering the commands and starting the coroutine.
        /// </summary>
        internal void Start()
        {
            this.RegisterVoteOptionsCommand();
            this.StartVoteCoroutine();
        }

        /// <summary>
        /// Stops the <see cref="Vote"/> by unregistering the command and stopping the coroutine.
        /// </summary>
        internal void Stop()
        {
            this.UnregisterVoteOptionsCommand();
            this.StopVoteCoroutine();
        }

        private IEnumerator<float> VoteCoroutine()
        {
            int timerCounter = 0;
            DisplayMessageHelper.DisplayFirstMessage(this.Question, out string firstMessage);
            yield return Timing.WaitForSeconds(5f);

            while (true)
            {
                if (timerCounter >= Config.VoteDuration + 1)
                {
                    VoteHandler.FinishVote();
                    yield break;
                }

                DisplayMessageHelper.DisplayWhileVoteMessage(firstMessage);
                timerCounter++;
                yield return Timing.WaitForSeconds(Config.RefreshInterval);
            }
        }

        private void RegisterVoteOptionsCommand()
        {
            foreach (VoteOption vote in this.VoteOptions)
            {
                vote.RegisterCommand();
            }
        }

        private void UnregisterVoteOptionsCommand()
        {
            foreach (VoteOption vote in this.VoteOptions)
            {
                vote.UnregisterCommand();
            }
        }

        private void StartVoteCoroutine()
        {
            this.voteCoroutine = Timing.RunCoroutine(this.VoteCoroutine());
        }

        private void StopVoteCoroutine()
        {
            Timing.KillCoroutines(this.voteCoroutine);
        }

        private long RandomNumber()
        {
            Random rng = new();
            return rng.Next(-1000, 1000);
        }
    }
}
