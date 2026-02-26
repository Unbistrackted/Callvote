#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System.Collections.Generic;
using Callvote.Patches;
using Callvote.API.Enums;
using Callvote.API.Features.Votes;

namespace Callvote.Features
{
    /// <summary>
    /// Represents the static handler that manages <see cref="Vote"/> queue and maximum called votes.
    /// Provides methods to manage and check the queue.
    /// </summary>
    public static class MaxVotesAndQueue
    {
        /// <summary>
        /// Gets queue of pending <see cref="Vote"/> instances. When queueing is enabled, new votes are placed here.
        /// </summary>
        public static Queue<Vote> VoteQueue { get; private set; } = new Queue<Vote>();

        /// <summary>
        /// Gets tracks how many <see cref="Vote"/> each player has initiated during the current round.
        /// Key: <see cref="Player"/> who called <see cref="Vote"/>. Value: number of <see cref="Vote"/> they've called.
        /// </summary>
        public static Dictionary<Player, int> PlayerCallVoteAmount { get; private set; } = [];

        /// <summary>
        /// Gets a value indicating whether the Queue is full.
        /// </summary>
        public static bool IsQueueFull => VoteQueue.Count >= CallvotePlugin.Instance.Config.QueueSize;

        /// <summary>
        /// Gets or sets a value indicating whether the vote queue will or will not start the next vote even if entries exist.
        /// </summary>
        public static bool IsQueuePaused { get; set; } = false;

        /// <summary>
        /// Attempts to start the next <see cref="Vote"/> in the <see cref="VoteQueue"/>. If no <see cref="Vote"/> is in progress and the <see cref="VoteQueue"/> is not paused and contains items,
        /// dequeues the next <see cref="Vote"/> and starts it.
        /// </summary>
        /// <returns>A <see cref="CallVoteStatus"/> representing if the <see cref="CallVoteStatus.VoteStarted"/>, or <see cref="CallVoteStatus.VoteEnqueued"/> .</returns>
        public static CallVoteStatus DequeueVote()
        {
            if (!VoteHandler.IsVoteActive && VoteQueue.Count != 0 && !IsQueuePaused)
            {
                try
                {
                    VoteHandlerPatch.IsDequeing = true;
                    VoteHandler.CallVote(VoteQueue.Dequeue());
                }
                finally
                {
                    VoteHandlerPatch.IsDequeing = false;
                }

                return CallVoteStatus.VoteStarted;
            }

            return CallVoteStatus.VoteEnqueued;
        }

        /// <summary>
        /// Checks if a Player is able to call a <see cref="Vote"/> based on per-player call limits in the config.
        /// </summary>
        /// <param name="player">Player to check if he is able to call a <see cref="Vote"/> .</param>
        /// <returns>If player is able to call a <see cref="Vote"/>.</returns>
        public static bool IsCallVoteAllowed(Player player)
        {
            if (!PlayerCallVoteAmount.ContainsKey(player))
            {
                PlayerCallVoteAmount.Add(player, 0);
            }

            PlayerCallVoteAmount[player]++;

            if (PlayerCallVoteAmount[player] > CallvotePlugin.Instance.Config.MaxAmountOfVotesPerRound &&
#if EXILED
                !player.CheckPermission("cv.bypass"))
#else
                !player.HasPermissions("cv.bypass"))
#endif
            {
                return false;
            }

            return true;
        }
    }
}
