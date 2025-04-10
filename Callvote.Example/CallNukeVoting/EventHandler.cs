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
                NukeVoting nukeVoting = new NukeVoting(Server.Host);
                VotingHandler.CallVoting(new CustomVoting(nukeVoting.CallVotePlayer, "Force remaining players to surface?", $"{CallNukeVoting.Instance.Prefix}.Nuke", nukeVoting.Callback, nukeVoting.Options));
            }
        }
    }
}
