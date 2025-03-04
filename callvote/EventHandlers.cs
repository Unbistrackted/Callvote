using Callvote.VoteHandlers;
using Exiled.Events.EventArgs.Player;
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
            VotingAPI.CurrentVoting.Stop();
        }

        public void OnPlayerJoined(JoinedEventArgs ev)
        {
            VotingAPI.ApplyCallvoteMenu(ev.Player);
        }
    }
}