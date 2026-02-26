#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.Features;
using HarmonyLib;
using Callvote.API.Enums;
using Callvote.API.Features.Votes;

namespace Callvote.Patches
{
#pragma warning disable SA1313
    /// <summary>
    /// Patch for adding Queue Functionality to Callvote.
    /// </summary>
    [HarmonyPatch(typeof(VoteHandler))]
    internal class VoteHandlerPatch
    {
#pragma warning disable SA1401 // Fields should be private
#pragma warning disable SA1600 // Elements should be documented
        internal static bool IsDequeing = false;

        [HarmonyPatch(nameof(VoteHandler.CallVote))]
        [HarmonyPrefix]
        private static bool CallVotePrefix(ref CallVoteStatus __result, Vote vote)
        {
            if (IsDequeing)
            {
                return true;
            }

            if (!MaxVotesAndQueue.IsCallVoteAllowed(Player.Get(vote.CallVotePlayer)))
            {
                __result = CallVoteStatus.MaxedCallVotes;
                Log.Info("entro aqui");
                return false;
            }

            if (CallvotePlugin.Instance.Config.EnableQueue)
            {
                if (MaxVotesAndQueue.IsQueueFull)
                {
                    __result = CallVoteStatus.QueueIsFull;
                    return false;
                }

                if (MaxVotesAndQueue.IsQueuePaused)
                {
                    __result = CallVoteStatus.QueueDisabled;
                    return false;
                }

                MaxVotesAndQueue.VoteQueue.Enqueue(vote);
                __result = MaxVotesAndQueue.DequeueVote();
                return false;
            }

            return true;
        }

        [HarmonyPatch(nameof(VoteHandler.FinishVote))]
        [HarmonyPostfix]
        private static void FinishVotePostix(bool isForced = true)
        {
            if (CallvotePlugin.Instance.Config.EnableQueue)
            {
                MaxVotesAndQueue.DequeueVote();
            }
        }
    }
}
