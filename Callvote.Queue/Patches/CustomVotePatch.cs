using Callvote.API.Enums;
using Callvote.Features.VoteTemplate;
using HarmonyLib;

namespace Callvote.Queue.Patches
{
#pragma warning disable SA1313
#pragma warning disable SA1600
    /// <summary>
    /// Patch for changing the Queue status return message.
    /// </summary>
    [HarmonyPatch(typeof(CustomVote))]
    internal class CustomVotePatch
    {
        [HarmonyPatch(nameof(CustomVote.GetMessageFromCallVoteStatus))]
        [HarmonyPrefix]
        private static bool CallVotePrefix(ref string __result, CallVoteStatus status)
        {
            __result = status switch
            {
                CallVoteStatus.VoteStarted => CallvotePlugin.Instance.Translation.VoteStarted,
                CallVoteStatus.VoteInProgress => CallvotePlugin.Instance.Translation.VoteInProgress,
                CallVoteStatus.MaxedCallVotes => CallvotePlugin.Instance.Translation.MaxVote,
                CallVoteStatus.VoteEnqueued => QueuePlugin.Instance.Translation.VoteEnqueued,
                CallVoteStatus.QueueIsFull => QueuePlugin.Instance.Translation.QueueIsFull,
                CallVoteStatus.QueueDisabled => QueuePlugin.Instance.Translation.QueueDisabled,
                _ => string.Empty,
            };

            return false;
        }
    }
}
