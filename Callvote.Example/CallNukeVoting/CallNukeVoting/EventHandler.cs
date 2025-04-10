using Callvote.API;
using Callvote.API.VotingsTemplate;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System.Linq;
namespace CallNukeVoting
{
    public class EventHandler
    {
        public void OnDied(DiedEventArgs ev)
        {
            if (!Round.AliveSides.Contains(Side.Scp) && Round.AliveSides.Count() >= 2)
            {
                VotingHandler.CallVoting(new CustomVoting(Server.Host, "Force remaining players to surface?", "CallNukeVoting.Nuke", new NukeVoting(Server.Host).Callback, new NukeVoting(Server.Host).Options));
            }
        }
    }
}
