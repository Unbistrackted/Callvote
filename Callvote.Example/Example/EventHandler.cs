using Callvote.API;
using Callvote.API.VotingsTemplate;
using Callvote.Features;
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
            ForcePlayersToSurface();
            KillScps(ev);
            ReviveSCPs(ev);
            VoteKickFFPlayer(ev);
        }

        private void ForcePlayersToSurface()
        {
            if (!Round.AliveSides.Contains(Side.Scp) && Round.AliveSides.Count() >= 2)
            {
                NukeVoting nukeVoting = new NukeVoting(Server.Host);
                CustomVoting forcePlayersToSurface = new CustomVoting(nukeVoting.CallVotePlayer, "Force remaining players to surface?", $"{CustomClass.Instance.Prefix}.Nuke", nukeVoting);
                VotingHandler.CallVoting(forcePlayersToSurface);
            }
        }

        private void VoteKickFFPlayer(DiedEventArgs ev)
        {
            if (ev.DamageHandler.IsFriendlyFire)
            {
                KickVoting voteKickFf = new KickVoting(Server.Host, ev.Attacker, "Killing Allies");
                VotingHandler.CallVoting(voteKickFf);
            }
        }

        private void ReviveSCPs(DiedEventArgs ev)
        {
            if (ev.Player.IsScp)
            {
                void callback(Voting vote)
                {
                    int yes = vote.Counter[Callvote.Callvote.Instance.Translation.CommandNo];
                    int no = vote.Counter[Callvote.Callvote.Instance.Translation.CommandYes];
                    if (yes > no)
                    {
                        ev.Player.RoleManager.ServerSetRole(ev.TargetOldRole, PlayerRoles.RoleChangeReason.None);
                        Map.Broadcast(5, $"{ev.TargetOldRole} respawned.");
                    }
                    Map.Broadcast(5, "The Voting Failed.");
                }
                BinaryVoting reviveSCP = new BinaryVoting(Server.Host, $"Revive {ev.TargetOldRole}?", $"{CustomClass.Instance.Prefix}.Respawn", callback);
                VotingHandler.CallVoting(reviveSCP);
            }
        }


        private void KillScps(DiedEventArgs ev)
        {
            if (ev.Attacker.IsScp)
            {
                string command = "win";
                VotingHandler.AddOptionToVoting(command, $"<color=green>{command}</color>");
                VotingHandler.AddOptionToVoting(Callvote.Callvote.Instance.Translation.OptionNo, Callvote.Callvote.Instance.Translation.CommandNo);
                void callback(Voting vote)
                {
                    int yes = vote.Counter[command];
                    int no = vote.Counter[Callvote.Callvote.Instance.Translation.CommandNo];
                    if (yes > no)
                    {
                        foreach (Player player in Player.List)
                        {
                            if (player.IsScp)
                            {
                                player.Kill("Get rekt lmao");
                                Map.Broadcast(5, "Humans ftw!!");
                            }
                        }
                    }
                }
                CustomVoting custom = new CustomVoting(Server.Host, "Kill all scps?", $"{CustomClass.Instance.Prefix}.Ezwin", callback);
                VotingHandler.CallVoting(custom);
            }
        }
    }
}
