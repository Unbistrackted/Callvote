using Callvote.API.Events;
using Callvote.API.Events.EventArgs;
using Callvote.ScpDiscord.Features;

namespace Callvote.ScpDiscord
{
    internal class EventHandler
    {
        internal EventHandler()
        {
            EventsHandlers.VoteEnded += this.OnVoteEnded;
        }

        ~EventHandler()
        {
            EventsHandlers.VoteEnded -= this.OnVoteEnded;
        }

        private void OnVoteEnded(VoteEndedEventArgs ev)
        {
            ScpDiscordEmbed.SendVoteResults(ev.Vote);
        }
    }
}
