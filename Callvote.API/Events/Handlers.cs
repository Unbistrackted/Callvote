using System;
using Callvote.API.Delegates;
using Callvote.API.Events.EventArgs;

namespace Callvote.API.Events
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Seft-explanatory.")]
    public static class Handlers
    {
        public static event CustomEventHandler<CalledVoteEventArgs> CalledVote;

        public static event CustomEventHandler<CallingVoteEventArgs> CallingVote;

        public static event CustomEventHandler<VotedEventArgs> Voted;

        public static event CustomEventHandler<VotingEventArgs> Voting;

        public static event CustomEventHandler<VoteEndedEventArgs> VoteEnded;

        public static event CustomEventHandler<VoteEndingEventArgs> VoteEnding;

        public static void OnCalledVote(CalledVoteEventArgs ev) => CalledVote?.InvokeEvent(ev);

        public static void OnCallingVote(CallingVoteEventArgs ev) => CallingVote?.InvokeEvent(ev);

        public static void OnVoted(VotedEventArgs ev) => Voted?.InvokeEvent(ev);

        public static void OnVoting(VotingEventArgs ev) => Voting?.InvokeEvent(ev);

        public static void OnVoteEnded(VoteEndedEventArgs ev) => VoteEnded?.InvokeEvent(ev);

        public static void OnVoteEnding(VoteEndingEventArgs ev) => VoteEnding?.InvokeEvent(ev);

        private static void InvokeEvent<T>(this CustomEventHandler<T> eventHandler, T args)
            where T : System.EventArgs
        {
            if (eventHandler == null)
            {
                return;
            }

            foreach (Delegate del in eventHandler.GetInvocationList())
            {
                if (del is not CustomEventHandler<T> customDelegate)
                {
                    continue;
                }

                try
                {
                    customDelegate(args);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
