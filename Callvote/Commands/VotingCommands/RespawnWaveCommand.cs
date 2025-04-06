using Callvote.API;
using Callvote.Features;
using CommandSystem;
using LabApi.Features.Wrappers;
using LabApi.Features.Permissions;
using Respawning;
using System;
using System.Linq;
using Callvote.Enums;

namespace Callvote.Commands.VotingCommands
{
    public class RespawnWaveCommand : ICommand
    {
        public string Command => "respawnwave";

        public string[] Aliases => new[] { "respawn", "wave" };

        public string Description => "Calls a respawn wave voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {

            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableRespawnWave)
            {
                response = Callvote.Instance.Translation.VoteRespawnWaveDisabled;
                return false;
            }

            if (!player.HasPermissions("cv.callvoterespawnwave"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitRespawnWave || !player.HasPermissions("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRespawnWave - Round.Duration.TotalSeconds}");
                return false;
            }

            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandMobileTaskForce, Callvote.Instance.Translation.OptionMtf);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandChaosInsurgency, Callvote.Instance.Translation.OptionCi);

            VotingHandler.CallVoting(
                Callvote.Instance.Translation.AskedToRespawn
                    .Replace("%Player%", player.Nickname),
                nameof(VotingType.RespawnWave),
                player,
                delegate (Voting vote)
                {
                    int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                    int mtfVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandMobileTaskForce] / (float)Player.List.Count() * 100f);
                    int ciVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandChaosInsurgency] / (float)Player.List.Count() * 100f);
                    if (mtfVotePercent >= Callvote.Instance.Config.ThresholdRespawnWave)
                    {
                        Server.SendBroadcast(Callvote.Instance.Translation.MtfRespawn
                            .Replace("%VotePercent%", mtfVotePercent + "%") , 5);
                        WaveManager.Spawn(WaveManager.Waves[0]);
                    }
                    else if (ciVotePercent >= Callvote.Instance.Config.ThresholdRespawnWave)
                    {
                        Server.SendBroadcast(Callvote.Instance.Translation.CiRespawn
                            .Replace("%VotePercent%", ciVotePercent.ToString()), 5);
                        WaveManager.Spawn(WaveManager.Waves[1]);
                    }
                    else
                    {
                        Server.SendBroadcast(Callvote.Instance.Translation.NoSuccessFullRespawn
                            .Replace("%VotePercent%", noVotePercent.ToString())
                            .Replace("%ThresholdRespawnWave%", Callvote.Instance.Config.ThresholdRespawnWave.ToString()), 5);
                    }
                });
            response = VotingHandler.Response;
            return true;
        }
    }
}