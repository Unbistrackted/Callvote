#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Callvote.API.Enums;
using Callvote.API.Features.Votes;
using Callvote.API.Interfaces;
using LabAPIPlayer = LabApi.Features.Wrappers.Player;

namespace Callvote.Features.VoteTemplate
{
    /// <summary>
    /// Represents the type that creates a <see cref="CustomVote"/>.
    /// </summary>
    public class CustomVote : Vote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVote"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that called the vote.</param>
        /// <param name="question">A <see cref="string"/> that represents the vote question.</param>
        /// <param name="voteType">A <see cref="string"/> that represents the vote type.</param>
        /// <param name="callback">A <see cref="Action{T}"/> that takes in a <see cref="Vote"/> that works as a callback.</param>
        /// <param name="options">A <see cref="HashSet{T}"/> that takes in a <see cref="VoteOption"/>.</param>
        /// <param name="players">A <see cref="HashSet{T}"/> that takes <see cref="Player"/>s that are only allowed to see and vote in a <see cref="Vote"/>. If null, gets all ready players instead.</param>
        public CustomVote(Player player, string question, string voteType, HashSet<VoteOption> options, Action<Vote> callback = null, HashSet<Player> players = null)
            : base(player.ReferenceHub, question, voteType, callback, options, players?.Select(p => (UserIndentifier)p.ReferenceHub).ToHashSet() ?? LabAPIPlayer.ReadyList.Select(p => (UserIndentifier)p.ReferenceHub).ToHashSet(), CallvotePlugin.Instance.Config.VoteDuration)
        {
            this.ResultsMessageDuration = CallvotePlugin.Instance.Config.FinalResultsDuration;
            this.RefreshInterval = CallvotePlugin.Instance.Config.RefreshInterval;
            this.MessageSize = CallvotePlugin.Instance.Config.MessageSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVote"/> class with the <see cref="Vote.VoteOptions"/> and <see cref="Vote.Callback"/> from a <see cref="IPredefinedVote"/>.
        /// </summary>
        /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
        /// <param name="question"><see cref="Vote.Question"/>.</param>
        /// <param name="voteType"><see cref="Vote.Type"/>.</param>
        /// <param name="voteTemplate">The <see cref="IPredefinedVote"/> to be copied from.</param>
        /// <param name="players">A <see cref="HashSet{T}"/> that takes <see cref="Player"/>s that are only allowed to see and vote in a <see cref="Vote"/>. If null, gets all ready players instead.</param>
        public CustomVote(Player player, string question, string voteType, IPredefinedVote voteTemplate, HashSet<Player> players = null)
            : base(player.ReferenceHub, question, voteType, voteTemplate.Callback, voteTemplate.VoteOptions, players?.Select(p => (UserIndentifier)p.ReferenceHub).ToHashSet() ?? LabAPIPlayer.ReadyList.Select(p => (UserIndentifier)p.ReferenceHub).ToHashSet(), duration: CallvotePlugin.Instance.Config.VoteDuration)
        {
            this.ResultsMessageDuration = CallvotePlugin.Instance.Config.FinalResultsDuration;
            this.RefreshInterval = CallvotePlugin.Instance.Config.RefreshInterval;
            this.MessageSize = CallvotePlugin.Instance.Config.MessageSize;
        }

        /// <inheritdoc/>
        public override string BuildCounterWithQuestionMessage()
        {
            if (!this.CanShowMessages)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = this.SbPool.Fetch();
            stringBuilder.Append($"{this.BuildQuestionMessage()}\n");

            foreach (VoteOption vote in this.VoteOptions)
            {
                stringBuilder.Append(CallvotePlugin.Instance.Translation.OptionAndCounter
                    .Replace("%VoteDetail%", vote.Detail)
                    .Replace("%VoteCounter%", this.Counter[vote].ToString()));
            }

            return this.SbPool.ToStringStore(stringBuilder);
        }

        /// <inheritdoc/>
        public override string BuildQuestionMessage()
        {
            if (!this.CanShowMessages)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = this.SbPool.Fetch();
            stringBuilder.Append(CallvotePlugin.Instance.Translation.AskedQuestion.Replace("%Question%", this.Question));

            int counter = 0;
            foreach (VoteOption voteOption in this.VoteOptions)
            {
                if (counter == 0)
                {
                    stringBuilder.Append($"|  {CallvotePlugin.Instance.Translation.Options.Replace("%VoteCommand%", voteOption.VoteCommand.Command).Replace("%VoteDetail%", voteOption.Detail)}  |");
                }
                else
                {
                    stringBuilder.Append($"  {CallvotePlugin.Instance.Translation.Options.Replace("%VoteCommand%", voteOption.VoteCommand.Command).Replace("%VoteDetail%", voteOption.Detail)} |");
                }

                counter++;
            }

            return this.SbPool.ToStringStore(stringBuilder);
        }

        /// <inheritdoc/>
        public override string BuildResultsMessage()
        {
            if (!this.CanShowMessages)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = this.SbPool.Fetch();
            stringBuilder.Append(CallvotePlugin.Instance.Translation.Results);

            foreach (VoteOption vote in this.VoteOptions)
            {
                stringBuilder.Append(CallvotePlugin.Instance.Translation.OptionAndCounter
                    .Replace("%VoteDetail%", vote.Detail)
                    .Replace("%VoteCounter%", this.Counter[vote].ToString()));
            }

            return this.SbPool.ToStringStore(stringBuilder);
        }

        /// <inheritdoc/>
        public override (bool Sucess, string Response)? VoteCommandResponse(UserIndentifier player, VoteOption voteOption)
        {
            if (!VoteHandler.IsVoteActive)
            {
                return (false, CallvotePlugin.Instance.Translation.NoVoteInProgress);
            }

            if (!VoteHandler.CurrentVote.AllowedPlayers.Contains(player) && player != (UserIndentifier)Server.Host.ReferenceHub)
            {
                return (false, CallvotePlugin.Instance.Translation.NoPermission);
            }

            if (!VoteHandler.CurrentVote.IsVoteOptionPresent(voteOption))
            {
                return (false, CallvotePlugin.Instance.Translation.NoOptionAvailable.Replace("%Option%", voteOption.VoteCommand.Command));
            }

            if (VoteHandler.CurrentVote.HasPlayerVotedOnOption(player, voteOption))
            {
                return (false, CallvotePlugin.Instance.Translation.AlreadyVoted.Replace("%Option%", voteOption.Detail));
            }

            if (!VoteHandler.CurrentVote.SubmitVoteOption(player, voteOption))
            {
                return (false, "Unable to place vote!");
            }

            return (true, CallvotePlugin.Instance.Translation.VoteAccepted.Replace("%Option%", voteOption.Detail));
        }

        /// <inheritdoc/>
        public override string GetMessageFromCallVoteStatus(CallVoteStatus status)
        {
            return status switch
            {
                CallVoteStatus.VoteStarted => CallvotePlugin.Instance.Translation.VoteStarted,
                CallVoteStatus.VoteInProgress => CallvotePlugin.Instance.Translation.VoteInProgress,
                CallVoteStatus.MaxedCallVotes => CallvotePlugin.Instance.Translation.MaxVote,
                _ => string.Empty,
            };
        }
    }
}