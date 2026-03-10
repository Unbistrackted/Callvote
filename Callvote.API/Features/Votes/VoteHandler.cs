using System;
using Callvote.API.Enums;
using Callvote.API.Events;
using Callvote.API.Events.EventArgs;
using Callvote.API.Features.Displays;
using LabApi.Events.Handlers;
using UnityEngine;

namespace Callvote.API.Features.Votes
{
    /// <summary>
    /// Represents the static handler that manages <see cref="Vote"/> lifecycle and the <see cref="Vote"/> queue.
    /// Provides methods to call, finish, enqueue, start <see cref="Vote"/>, add options, and many more.
    /// </summary>
    public static class VoteHandler
    {
        static VoteHandler()
        {
            if (Application.productName == "SCPSL")
            {
                ServerEvents.RoundRestarted += () => FinishVote(true);
                ServerEvents.WaitingForPlayers += () => FinishVote(true);
            }
        }

        /// <summary>
        /// Gets the currently active <see cref="Vote"/> instance. Null when no vote is in progress.
        /// </summary>
        public static Vote CurrentVote { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Vote"/> is currently active.
        /// </summary>
        public static bool IsVoteActive => CurrentVote != null;

        /// <summary>
        /// Gets or sets a value indicating whether the Discord Webhook will be able to send a webhook message.
        /// </summary>
        public static bool ShouldSendWebhookMessage { get; set; } = false;

        /// <summary>
        /// Request to start a <see cref="Vote"/>.
        /// If queueing is enabled the <paramref name="vote"/> will be enqueued and the <see cref="Vote"/> is started immediately.
        /// </summary>
        /// <param name="vote">The <see cref="Vote"/> to start or enqueue.</param>
        /// <returns>A <see cref="CallVoteStatus"/> representing if the action was sucessfull, or for example, if the queue is full.</returns>
        public static CallVoteStatus CallVote(Vote vote)
        {
            if (vote == null)
            {
                throw new ArgumentNullException(nameof(vote), "Vote cannot be null!");
            }

            if (!IsVoteActive)
            {
                CallingVoteEventArgs e = new(vote);
                EventsHandlers.OnCallingVote(e);
                if (!e.IsAllowed)
                {
                    return CallVoteStatus.VoteCancelled;
                }

                CurrentVote = vote;
                CurrentVote.Start();
                CalledVoteEventArgs ev = new(CurrentVote);
                EventsHandlers.OnCalledVote(ev);
                return CallVoteStatus.VoteStarted;
            }

            return CallVoteStatus.VoteInProgress;
        }

        /// <summary>
        /// Finishes and clears the active <see cref="Vote"/>.
        /// Stops the vote, displays results (or invokes a callback when provided).
        /// </summary>
        /// <param name="isForced">If the voting will display the results message or invoke the Callback.</param>
        public static void FinishVote(bool isForced = false)
        {
            if (!IsVoteActive)
            {
                return;
            }

            VoteEndingEventArgs e = new(CurrentVote);
            if (!e.IsAllowed)
            {
                return;
            }

            CurrentVote?.Stop();

            if (!isForced)
            {
                if (CurrentVote?.Callback == null)
                {
                    DisplayHandler.Show(CurrentVote.ResultsMessageDuration, CurrentVote.BuildResultsMessage(), CurrentVote.AllowedPlayers);
                }
                else
                {
                    CurrentVote?.Callback.Invoke(CurrentVote);
                }
            }

            CurrentVote = null;
            VoteEndedEventArgs ev = new(CurrentVote);
            EventsHandlers.OnVoteEnded(ev);
        }

        /// <summary>
        /// Creates a <see cref="VoteOption"/>.
        /// </summary>
        /// <param name="option">The <see cref="VoteOption"/> Option.</param>
        /// <param name="detail">The <see cref="VoteOption"/> Detail.</param>
        /// <returns>A new a <see cref="VoteOption"/>.</returns>
        public static VoteOption CreateVoteOption(string option, string detail)
        {
            return new VoteOption(option, detail);
        }
    }
}
