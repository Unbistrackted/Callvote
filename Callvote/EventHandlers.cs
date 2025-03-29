using Callvote.VoteHandlers;
using Exiled.Events.EventArgs.Server;

namespace Callvote
{
    public class EventHandlers
    {
        public void OnWaitingForPlayers()
        {
            if (Callvote.Instance.Config.EnableQueue) { VotingHandler.VotingQueue.Clear(); }
            VotingHandler.FinishVoting();
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (Callvote.Instance.Config.EnableQueue) { VotingHandler.VotingQueue.Clear(); }
            VotingHandler.FinishVoting();
        }
    }
}
