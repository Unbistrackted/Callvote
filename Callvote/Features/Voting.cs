#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Callvote.API;
using Callvote.Commands.VotingCommands;
using CommandSystem;
using MEC;
using RemoteAdmin;

namespace Callvote.Features
{
    /// <summary>
    /// Represents the type that manages and creates the voting.
    /// </summary>
    public class Voting
    {
        private readonly Dictionary<string, string> playerVote;
        private CoroutineHandle votingCoroutine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Voting"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that called the voting.</param>
        /// <param name="question">A <see cref="string"/> that represents the voting question.</param>
        /// <param name="votingType">A <see cref="string"/> that represents the voting type.</param>
        /// <param name="callback">A <see cref="Action{T}"/> that takes in a <see cref="Voting"/> that works as a callback.</param>
        /// <param name="options">A <see cref="Dictionary{Option, Detail}"/> that takes in a <see cref="string"/> option as key and a <see cref="string"/> detail as value. </param>
        /// <param name="players">A <see cref="IEnumerable{Player}"/> that takes <see cref="Player"/>s that are only allowed to see and vote in a <see cref="Voting"/>.</param>
        internal Voting(Player player, string question, string votingType, Action<Voting> callback, Dictionary<string, string> options = null, IEnumerable<Player> players = null)
        {
            this.CallVotePlayer = player;
            this.Question = question;
            this.Callback = callback;
            this.Options = new Dictionary<string, string>(options ?? VotingHandler.Options);
#if EXILED
            AllowedPlayers = [.. players ?? Player.List.Where(p => p.ReferenceHub.nicknameSync.NickSet && p.ReferenceHub.authManager.InstanceMode != 0)];
#else
            this.AllowedPlayers = [.. players ?? Player.ReadyList];
#endif
            this.playerVote = [];
            this.CommandList = [];
            this.Counter = new ConcurrentDictionary<string, int>();
            this.votingCoroutine = default(CoroutineHandle);

            foreach (string option in this.Options.Keys)
            {
                this.Counter[option] = 0;
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
        /// Gets the Dictionary of options with their details in the <see cref="Voting"/> .
        /// Key: Command/Option name. Value: Option/Label name for the command.
        /// </summary>
        public Dictionary<string, string> Options { get; init; }

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
        /// Gets the ammount of votes in a <see cref="Voting"/>.
        /// Key: Command/Option name. Value: Ammount of votes.
        /// </summary>
        public ConcurrentDictionary<string, int> Counter { get; }

        /// <summary>
        /// Gets the commands registered in a <see cref="Voting"/>..
        /// /// Key: Command/Option name. Value: The registered <see cref="ICommand"/>.
        /// </summary>
        public Dictionary<string, ICommand> CommandList { get; }

        /// <summary>
        /// Makes a <see cref="Player"/> vote on a <see param="option"/> of a <see cref="Voting"/>.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> who is will be voting.</param>
        /// <param name="option">The option that will be voted.</param>
        /// <returns>A <see cref="string"/> representing if the votes was sucessful or not.</returns>
        /// <remarks>
        /// The vote will only go through if the <see cref="votingCoroutine"/> is active.
        /// </remarks>
        public string Vote(Player player, string option)
        {
            if (!votingCoroutine.IsRunning)
            {
                return CallvotePlugin.Instance.Translation.NoVotingInProgress;
            }

            if (!this.AllowedPlayers.Contains(player))
            {
                return CallvotePlugin.Instance.Translation.NoPermission;
            }

            string playerUserId = player.UserId;

            if (!this.Options.ContainsKey(option))
            {
                return CallvotePlugin.Instance.Translation.NoOptionAvailable.Replace("%Option%", option);
            }

            if (this.playerVote.ContainsKey(playerUserId))
            {
                if (this.playerVote[playerUserId] == option)
                {
                    return CallvotePlugin.Instance.Translation.AlreadyVoted;
                }

                this.Counter.AddOrUpdate(this.playerVote[playerUserId], 0, (key, value) => Math.Max(0, value - 1));
                this.playerVote[playerUserId] = option;
            }
            else
            {
                this.playerVote.Add(playerUserId, option);
            }

            this.Counter.AddOrUpdate(option, 1, (key, value) => value + 1);

            return CallvotePlugin.Instance.Translation.VoteAccepted.Replace("%Option%", this.Options[option]);
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
            if (!this.Counter.ContainsKey(option))
            {
                return CallvotePlugin.Instance.Translation.NoOptionAvailable.Replace("%Option%", option);
            }

            this.Counter.AddOrUpdate(option, amount, (key, value) => value + amount);
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
                return CallvotePlugin.Instance.Translation.MaxVote;
            }

            this.RegisterVoteCommands();
            this.StartVotingCoroutine();

            return CallvotePlugin.Instance.Translation.VotingStarted;
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
                if (timerCounter >= CallvotePlugin.Instance.Config.VoteDuration + 1)
                {
                    VotingHandler.FinishVoting();
                    yield break;
                }

                DisplayMessageHelper.DisplayWhileVotingMessage(firstMessage);
                timerCounter++;
                yield return Timing.WaitForSeconds(CallvotePlugin.Instance.Config.RefreshInterval);
            }
        }

        private void RegisterVoteCommands()
        {
            bool alreadyRegistered = false;

            foreach (KeyValuePair<string, string> kvp in this.Options)
            {
                if (QueryProcessor.DotCommandHandler.TryGetCommand(kvp.Key, out ICommand existingCommand))
                {
                    alreadyRegistered = true;
                }
            }

            foreach (KeyValuePair<string, string> kvp in this.Options)
            {
                VoteCommand voteCommand = new(kvp.Key);

                if (alreadyRegistered)
                {
                    this.Options.Remove(kvp.Key);
                    this.Options.Add("cv" + kvp.Key, kvp.Value);
                    this.Counter.TryRemove(kvp.Key, out _);
                    this.Counter.TryAdd("cv" + kvp.Key, 0);
                    voteCommand.Command = "cv" + kvp.Key;
                }

                this.CommandList.Add(kvp.Key, voteCommand);
                QueryProcessor.DotCommandHandler.RegisterCommand(voteCommand);
            }
        }

        private void UnregisterVoteCommands()
        {
            foreach (KeyValuePair<string, ICommand> command in this.CommandList)
            {
                QueryProcessor.DotCommandHandler.UnregisterCommand(command.Value);
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