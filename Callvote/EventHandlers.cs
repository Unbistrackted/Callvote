using Callvote.VoteHandlers;
using Exiled.Events.EventArgs.Server;

namespace Callvote
{
    public class EventHandlers
    {
        public void OnWaitingForPlayers()
        {
            if (VotingAPI.CurrentVoting != null) VotingAPI.CurrentVoting.Stop();
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (VotingAPI.CurrentVoting != null) VotingAPI.CurrentVoting.Stop();
        }
    }
}
