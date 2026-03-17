using Callvote.API.Events;
using Callvote.API.Events.EventArgs;
using Callvote.DiscordLab.Features;

namespace Callvote.DiscordLab
{
    internal class EventHandler
    {
        internal EventHandler()
        {
            EventsHandlers.VoteEnded += OnVoteEnded;
        }

        ~EventHandler()
        {
            EventsHandlers.VoteEnded -= OnVoteEnded;
        }

        private void OnVoteEnded(VoteEndedEventArgs ev)
        {
            DiscordLabEmbed.SendVoteResults(ev.Vote);
        }
    }
}
