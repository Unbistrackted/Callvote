using Callvote.VoteHandlers;
using Exiled.Events.EventArgs.Server;
using GameCore;
using MEC;
using PluginAPI.Events;

namespace Callvote
{
    public class EventHandlers
    {
        public void OnWaitingForPlayers()
        {
            if (VoteAPI.CurrentVote != null && VoteAPI.DictionaryOfVotes != null)
            {
                VoteAPI.CurrentVote.Timer.Stop();
                VoteAPI.CurrentVote.Timer.Dispose();
                VoteAPI.DictionaryOfVotes.Clear();
            }
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            VoteAPI.StopVote();
        }
    }
}