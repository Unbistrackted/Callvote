using UnityEngine;

namespace Callvote.API.Features.Votes
{
#pragma warning disable SA1600 // Elements should be documented
    internal class VoteCoroutineMonoBehaviour : MonoBehaviour
    {
        internal Vote Vote { get; set; }

        internal Coroutine VoteCoroutine { get; private set; }

        private void OnDestroy()
        {
            if (this.VoteCoroutine == default || this.Vote == null)
            {
                return;
            }

            this.StopCoroutine(this.VoteCoroutine);
            this.VoteCoroutine = default;
            this.Vote = null;
        }

        private void Start()
        {
            if (this.Vote == null)
            {
                return;
            }

            this.VoteCoroutine = this.StartCoroutine(this.Vote.VoteCoroutine());
        }
    }
}
