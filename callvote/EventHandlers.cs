using Callvote.VoteHandlers;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;

namespace Callvote
{
    public class EventHandlers
    {
        public void OnWaitingForPlayers()
        {
            if (VoteAPI.CurrentVoting != null) VoteAPI.CurrentVoting.Stop();
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            VoteAPI.CurrentVoting.Stop();
        }

        public void OnPlayerJoined(JoinedEventArgs ev)
        {
            VoteAPI.ApplyCallvoteMenu(ev.Player);
        }
    }
}