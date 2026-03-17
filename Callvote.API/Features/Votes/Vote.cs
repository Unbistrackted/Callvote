using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Callvote.API.Enums;
using Callvote.API.Events;
using Callvote.API.Events.EventArgs;
using Callvote.API.Features.Commands;
using Callvote.API.Features.Displays;
using Callvote.API.Features.Pooling;
using Callvote.API.Interfaces;
using UnityEngine;

namespace Callvote.API.Features.Votes
{
    /// <summary>
    /// Represents the type that manages and creates the <see cref="Vote"/>.
    /// Responsible for the <see cref="Vote"/> cycle, and managing the <see cref="VoteOption"/>s.
    /// </summary>
    public class Vote
    {
        private GameObject coroutineGameObject;
        private VoteCoroutineMonoBehaviour coroutine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vote"/> class.
        /// </summary>
        /// <param name="player">The <see cref="UserIndentifier"/> that called the vote.</param>
        /// <param name="question">The question.</param>
        /// <param name="voteType">The vote type.</param>
        /// <param name="callback">The vote's callback when it's finished.</param>
        /// <param name="voteOptions">The Hashset containing the <see cref="VoteOptions"/>.</param>
        /// <param name="players">The Hashset of <see cref="UserIndentifier"/>s that will be able to see and vote.</param>
        /// <param name="duration">The voting duration.</param>
        public Vote(UserIndentifier player, string question, string voteType, Action<Vote> callback, HashSet<VoteOption> voteOptions, HashSet<UserIndentifier> players, float duration = 30)
        {
            this.CallVotePlayer = player;
            this.Question = question;
            this.Type = voteType;
            this.Duration = duration;
            this.Callback = callback;
            this.VoteOptions = voteOptions ?? new HashSet<VoteOption>();
            this.AllowedPlayers = players ?? new HashSet<UserIndentifier>();
            this.VoteId = DateTime.UtcNow.Ticks;

            foreach (VoteOption vote in this.VoteOptions)
            {
                this.Counter[vote] = 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vote"/> class using a <see cref="IPredefinedVote"/>.
        /// </summary>
        /// <param name="player">The <see cref="UserIndentifier"/> that called the vote.</param>
        /// <param name="question">The question.</param>
        /// <param name="voteType">The vote type.</param>
        /// <param name="predefinedVote">The predefined vote.</param>
        /// <param name="players">The Hashset of <see cref="UserIndentifier"/>s that will be able to see and vote.</param>
        /// <param name="duration">The voting duration.</param>
        public Vote(UserIndentifier player, string question, string voteType, IPredefinedVote predefinedVote, HashSet<UserIndentifier> players = null, float duration = 30)
            : this(player, question, voteType, predefinedVote.Callback, predefinedVote.VoteOptions, players, duration)
        {
        }

        /// <summary>
        /// Gets the pool of StringBuilders.
        /// </summary>
        public StringBuilderPool SbPool { get; } = new StringBuilderPool(2);

        /// <summary>
        /// Gets the player who called the <see cref="Vote"/> .
        /// </summary>
        public UserIndentifier CallVotePlayer { get; }

        /// <summary>
        /// Gets the <see cref="Vote"/> question.
        /// </summary>
        public string Question { get; }

        /// <summary>
        /// Gets the <see cref="Vote"/> type.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the <see cref="Vote"/> duration.
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// Gets the <see cref="Vote"/> callback.
        /// </summary>
        public Action<Vote> Callback { get; }

        /// <summary>
        /// Gets the allowed players that can see and vote on the <see cref="Vote"/> .
        /// </summary>
        public HashSet<UserIndentifier> AllowedPlayers { get; }

        /// <summary>
        /// Gets a dictionary of <see cref="VoteCommand"/> that have been registered, indexed by their command names.
        /// </summary>
        public Dictionary<string, VoteCommand> RegisteredCommands { get; } = [];

        /// <summary>
        /// Gets the <see cref="HashSet{Vote}"/> with the available <see cref="VoteOption"/>s in the <see cref="Vote"/> .
        /// </summary>
        /// <remarks><see cref="VoteOption"/>s can have the same <see cref="VoteOption.Option"/>, but not the same Command.</remarks>
        public HashSet<VoteOption> VoteOptions { get; }

        /// <summary>
        /// Gets or sets the <see cref="Vote"/> refresh interval.
        /// </summary>
        public float RefreshInterval { get; set; } = 1;

        /// <summary>
        /// Gets or sets the <see cref="Vote"/> message letter size.
        /// </summary>
        /// <remarks>If the size is 0, it will use Callvote's message sizing method - <see cref="DisplayHandler.CalculateMessageSize(string)"/>.</remarks>
        public int MessageSize { get; set; } = 0;

        /// <summary>
        /// Gets or sets the <see cref="Vote"/> inital message duration.
        /// </summary>
        public float InitialMessageDuration { get; set; } = 5;

        /// <summary>
        /// Gets or sets the <see cref="Vote"/> results message duration.
        /// </summary>
        public float ResultsMessageDuration { get; set; } = 5;

        /// <summary>
        /// Gets or sets a value indicating whether it can show messages.
        /// </summary>
        public bool CanShowMessages { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether multiple votes are allowed for the same player.
        /// </summary>
        public bool IsMultipleVotesAllowed { get; set; } = false;

        /// <summary>
        /// Gets the <see cref="Vote"/> ID.
        /// </summary>
        public long VoteId { get; }

        /// <summary>
        /// Gets the Dictionary of Players with their <see cref="VoteOption"/> in the <see cref="Vote"/> .
        /// Key: Player. Value: <see cref="VoteOption"/>.
        /// </summary>
        public ConcurrentDictionary<UserIndentifier, HashSet<VoteOption>> PlayerVote { get; } = new();

        /// <summary>
        /// Gets the ammount of votes of a <see cref="VoteOption"/> in a <see cref="Vote"/>.s
        /// Key: <see cref="VoteOption"/>. Value: Ammount of votes.
        /// </summary>
        public ConcurrentDictionary<VoteOption, int> Counter { get; private set; } = new();

        /// <summary>
        /// Gets a value indicating whether the <see cref="Vote"/> coroutine is active.
        /// </summary>
        public bool IsCoroutineActive => this.coroutine != null;

        /// <summary>
        /// Makes a <see cref="UserIndentifier"/> vote on a <see cref="VoteOption"/> of a <see cref="Vote"/>.
        /// </summary>
        /// <param name="user">The <see cref="UserIndentifier"/> who is will be submiting the vote option.</param>
        /// <param name="voteOption">The <see cref="VoteOption"/> that will be selected.</param>
        /// <returns>A <see cref="bool"/> representing if the vote process was sucessful or not.</returns>
        /// <remarks>
        /// The vote will only go through if the <see cref="IsCoroutineActive"/>.
        /// </remarks>
        public bool SubmitVoteOption(UserIndentifier user, VoteOption voteOption)
        {
            if (!this.IsCoroutineActive || !this.AllowedPlayers.Contains(user) || !this.IsVoteOptionPresent(voteOption))
            {
                return false;
            }

            VotingEventArgs e = new(this, voteOption);
            EventsHandlers.OnVoting(e);
            if (!e.IsAllowed)
            {
                return false;
            }

            // So I wanted to make this comment cause AddOrUpdate is so fucking cool and useful like bro DUGYHADHUJBGAHGYDAHDA, props to it
            this.PlayerVote.AddOrUpdate(
                user,
                (_) =>
                {
                    this.Counter.AddOrUpdate(voteOption, 1, (_, ammount) => ammount + 1);
                    return [voteOption];
                },
                (_, votes) =>
                {
                    // Need to lock cause hashset is not thread safe and I don't feel like using a concurrent dictionary with a random ass value
                    lock (votes)
                    {
                        if (votes.Contains(voteOption))
                        {
                            if (this.IsMultipleVotesAllowed)
                            {
                                // Removes the votes if player already voted on the same option.
                                this.Counter.AddOrUpdate(voteOption, 0, (_, ammount) => Math.Max(0, ammount - 1));
                                votes.Remove(voteOption);
                            }

                            return votes;
                        }

                        if (!this.IsMultipleVotesAllowed)
                        {
                            // Since there's only one vote inside of the hashset when multiple votes are not allowed, the .first() is always getting the one that I want
                            this.Counter.AddOrUpdate(votes.First(), 0, (_, ammount) => Math.Max(0, ammount - 1));
                            votes.Remove(votes.First());
                            this.Counter.AddOrUpdate(voteOption, 1, (_, ammount) => ammount + 1);
                        }
                        else
                        {
                            this.Counter.AddOrUpdate(voteOption, 1, (_, ammount) => ammount + 1);
                        }

                        votes.Add(voteOption);

                        return votes;
                    }
                });

            VotedEventArgs ev = new(this, voteOption);
            EventsHandlers.OnVoted(ev);
            return true;
        }

        /// <summary>
        /// Gets the <see cref="VoteOption"/> in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="command">The command that will be searched for.</param>
        /// <returns>A <see cref="VoteOption"/> if found, otherwise null.</returns>
        public VoteOption GetVoteOptionFromCommand(string command) => this.RegisteredCommands.TryGetValue(command, out VoteCommand voteCommand) ? voteCommand.VoteOption : null;

        /// <summary>
        /// Gets a value indicating whether the <see cref="UserIndentifier"/> has already voted on a specific <see cref="VoteOption"/> in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="user">The user that is going to checked if he voted on a option or not.</param>
        /// <param name="voteOption">The vote option.</param>
        /// <returns>If the user voted in a vote option.</returns>
        public bool HasPlayerVotedOnOption(UserIndentifier user, VoteOption voteOption) => this.PlayerVote.TryGetValue(user, out HashSet<VoteOption> votes) && votes.Contains(voteOption);

        /// <summary>
        /// Gets the <see cref="VoteOption"/> in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="option">The option that will be searched for.</param>
        /// <returns>A <see cref="HashSet{Vote}"/> .</returns>
        /// <remarks><see cref="VoteOption"/>s in <see cref="VoteOptions"/> can have the same <see cref="VoteOption.Option"/>, consider using <see cref="GetVoteOptionFromCommand"/> if you made sure the command is not already registed by another plugin.</remarks>
        public HashSet<VoteOption> GetVoteOptions(string option) => [.. this.VoteOptions.Where(vote => vote.Option == option)];

        /// <summary>
        /// Gets the <see cref="VoteOption"/> in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="command">The string option that will be searched for.</param>
        /// <param name="vote">Returns a <see cref="VoteOption"/> if found, otherwise null.</param>
        /// <returns>If the specific <see cref="VoteOption"/> was found.</returns>
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
        /// Checks if a <see cref="VoteOption"/> exists in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="vote">The <see cref="VoteOption"/> that will be searched for.</param>
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
        /// Gets a <see cref="VoteOption"/> percentage based on <see cref="AllowedPlayers"/> in a <see cref="Vote"/> .
        /// </summary>
        /// <param name="voteOption">The <see cref="VoteOption"/> that will be searched for.</param>
        /// <returns>A int value as a percentage.</returns>
        public int GetVoteOptionPercentage(VoteOption voteOption)
        {
            if (!this.IsVoteOptionPresent(voteOption))
            {
                return 0;
            }

            if (this.AllowedPlayers == null || this.AllowedPlayers.Count == 0)
            {
                return 0;
            }

            if (!this.Counter.TryGetValue(voteOption, out int count))
            {
                return 0;
            }

            return Mathf.Min((int)(count / (float)this.AllowedPlayers.Count * 100f), 100);
        }

        /// <summary>
        /// Gets the <see cref="VoteOption"/> that has the most ammount of votes .
        /// </summary>
        /// <returns>Ai<see cref="VoteOption"/> with the most ammount of votes in this <see cref="Vote"/>.</returns>
        public VoteOption GetWinningVoteOption() => this.Counter.Count == 0 ? null : this.Counter.OrderByDescending(x => x.Value).First().Key;

        /// <summary>
        /// Rigs the <see cref="Vote"/> <see cref="Counter"/> .
        /// </summary>
        /// <param name="option">The option that will be rigged.</param>
        /// <param name="vote">The <see cref="VoteOption"/> from the <see cref="Vote"/>.</param>
        /// <param name="amount">The ammount of votes added to that option.</param>
        /// <returns>If the vote rigging was sucessful or not.</returns>
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
        /// The handler method for the status message corresponding to the specified <see cref="CallVoteStatus"/>.
        /// </summary>
        /// <param name="status">The <see cref="CallVoteStatus"/> for which to retrieve the message.</param>
        /// <returns>A string containing the message for the given status; returns an empty string if the status is not
        /// recognized.</returns>
        public virtual string GetMessageFromCallVoteStatus(CallVoteStatus status)
        {
            return status switch
            {
                CallVoteStatus.VoteStarted => "Vote has been started!",
                CallVoteStatus.VoteInProgress => "A vote is currently in progress.",
                _ => "Something Went Wrong!",
            };
        }

        /// <summary>
        /// The default handler for the command response.
        /// </summary>
        /// <param name="player">The player who sent the vote command.</param>
        /// <param name="voteOption">The option that the player tried to vote on.</param>
        /// <returns>A tuple representing if it was sucessfull and the response.</returns>
        public virtual (bool Sucess, string Response)? VoteCommandResponse(UserIndentifier player, VoteOption voteOption)
        {
            if (!VoteHandler.IsVoteActive)
            {
                return (false, "There is no vote in progress.");
            }

            if (!this.AllowedPlayers.Contains(player))
            {
                return (false, "You do not have permission to run this command!");
            }

            if (!this.IsVoteOptionPresent(voteOption))
            {
                return (false, $"Vote does not have the option {voteOption.Option}.");
            }

            if (this.HasPlayerVotedOnOption(player, voteOption))
            {
                return (false, $"You've already voted on {voteOption.Detail}.");
            }

            if (!this.SubmitVoteOption(player, voteOption))
            {
                return (false, "Unable to place vote!");
            }

            return (true, $"You voted {voteOption.Detail}");
        }

        /// <summary>
        /// Builds a string with the initial vote message.
        /// </summary>
        /// <returns>The string that was formated.</returns>
        public virtual string BuildQuestionMessage()
        {
            if (!this.CanShowMessages)
            {
                return string.Empty;
            }

            int counter = 0;
            StringBuilder stringBuilder = this.SbPool.Fetch();
            stringBuilder.Append($"{this.Question}\n");

            foreach (VoteOption voteOption in this.VoteOptions)
            {
                string optionString = $".{voteOption.VoteCommand.Command} = {voteOption.Detail}";
                if (counter == 0)
                {
                    stringBuilder.Append($"|  {optionString}  |");
                }
                else
                {
                    stringBuilder.Append($"  {optionString}  |");
                }

                counter++;
            }

            return this.SbPool.ToStringStore(stringBuilder);
        }

        /// <summary>
        /// Builds a string with the counter vote message.
        /// </summary>
        /// <returns>The string that was formated.</returns>
        public virtual string BuildCounterWithQuestionMessage()
        {
            if (!this.CanShowMessages)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = this.SbPool.Fetch();
            stringBuilder.Append($"{this.BuildQuestionMessage()}\n");

            foreach (VoteOption voteOption in this.VoteOptions)
            {
                if (!this.Counter.TryGetValue(voteOption, out int count))
                {
                    continue;
                }

                stringBuilder.Append($" {voteOption.Detail} ({count}) ");
            }

            return this.SbPool.ToStringStore(stringBuilder);
        }

        /// <summary>
        /// Builds a string with the counter vote message.
        /// </summary>
        /// <returns>The string that was formated.</returns>
        public virtual string BuildResultsMessage()
        {
            if (!this.CanShowMessages)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = this.SbPool.Fetch();
            stringBuilder.Append($"Final results:\n");

            foreach (VoteOption voteOption in this.VoteOptions)
            {
                if (!this.Counter.TryGetValue(voteOption, out int count))
                {
                    continue;
                }

                stringBuilder.Append($" {voteOption.Detail} ({count}) ");
            }

            return this.SbPool.ToStringStore(stringBuilder);
        }

        /// <summary>
        /// Starts the <see cref="Vote"/> by registering the commands and starting the coroutine.
        /// </summary>
        /// <returns>Returns the <see cref="CallVoteStatus"/>.</returns>
        internal CallVoteStatus StartVote()
        {
            CallingVoteEventArgs e = new(this);
            EventsHandlers.OnCallingVote(e);
            if (!e.IsAllowed)
            {
                return CallVoteStatus.VoteCancelled;
            }

            this.RegisterAllVoteOptions();
            this.StartVoteCoroutine();

            CalledVoteEventArgs ev = new(this);
            EventsHandlers.OnCalledVote(ev);
            return CallVoteStatus.VoteStarted;
        }

        /// <summary>
        /// Stops the <see cref="Vote"/> by unregistering the command and stopping the coroutine.
        /// </summary>
        /// <param name="isForced">If the voting will display the results message or invoke the Callback.</param>
        internal void FinishVote(bool isForced = false)
        {
            VoteEndingEventArgs e = new(this);
            if (!e.IsAllowed)
            {
                return;
            }

            this.UnregisterVoteOptionsCommand();
            this.StopVoteCoroutine();

            if (!isForced)
            {
                return;
            }

            if (this.Callback == null)
            {
                DisplayHandler.Show(this.ResultsMessageDuration, this.BuildResultsMessage(), this.AllowedPlayers);
            }
            else
            {
                this?.Callback.Invoke(this);
            }

            // Prevents being null when it's later used in the event
            Vote vote = this;
            VoteEndedEventArgs ev = new(vote);
            EventsHandlers.OnVoteEnded(ev);
        }

        /// <summary>
        /// Manages the voting process by displaying the voting question and updating the countdown timer until the
        /// voting period ends.
        /// </summary>
        /// <returns>An enumerator that yields control at intervals until the voting duration has elapsed.</returns>
        internal IEnumerator VoteCoroutine()
        {
            float timerCounter = 0f;
            DisplayHandler.Show(this.InitialMessageDuration, this.BuildQuestionMessage(), this.AllowedPlayers);
            yield return new WaitForSeconds(this.InitialMessageDuration);

            while (true)
            {
                if (timerCounter >= this.Duration)
                {
                    VoteHandler.FinishVote();
                    yield break;
                }

                DisplayHandler.Show(this.RefreshInterval, this.BuildCounterWithQuestionMessage(), this.AllowedPlayers);
                timerCounter += this.RefreshInterval;
                yield return new WaitForSeconds(this.RefreshInterval);
            }
        }

        private void RegisterAllVoteOptions()
        {
            foreach (VoteOption voteOption in this.VoteOptions)
            {
                CommandHandler.RegisterCommand(voteOption.VoteCommand);
                this.RegisteredCommands[voteOption.VoteCommand.Command] = voteOption.VoteCommand;
            }
        }

        private void UnregisterVoteOptionsCommand()
        {
            foreach (KeyValuePair<string, VoteCommand> commandKvp in this.RegisteredCommands)
            {
                CommandHandler.UnregisterCommand(commandKvp.Value);
            }
        }

        private void StartVoteCoroutine()
        {
            if (this.IsCoroutineActive)
            {
                return;
            }

            this.coroutineGameObject = new GameObject($"VoteCoroutine_{this.VoteId}");
            this.coroutine = this.coroutineGameObject.AddComponent<VoteCoroutineMonoBehaviour>();
            this.coroutine.Vote = this;
        }

        private void StopVoteCoroutine()
        {
            if (!this.IsCoroutineActive)
            {
                return;
            }

            UnityEngine.Object.Destroy(this.coroutineGameObject);
        }
    }
}
