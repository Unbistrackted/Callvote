﻿using System;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Respawning;

namespace Callvote.Commands
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

            if (!player.CheckPermission("cv.callvoterespawnwave"))
            {
                response = Callvote.Instance.Translation.NoPermissionToVote;
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitRespawnWave || !player.CheckPermission("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRespawnWave - Round.ElapsedTime.TotalSeconds}");
                return false;
            }

            VotingAPI.Options.Add(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            VotingAPI.Options.Add(Callvote.Instance.Translation.CommandMobileTaskForce, Callvote.Instance.Translation.OptionMtf);
            VotingAPI.Options.Add(Callvote.Instance.Translation.CommandChaosInsurgency, Callvote.Instance.Translation.OptionCi);

            VotingAPI.CurrentVoting = new Voting(Callvote.Instance.Translation.AskedToRespawn
                    .Replace("%Player%", player.Nickname),
                VotingAPI.Options,
                player,
                delegate(Voting vote)
                {
                    int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                    int mtfVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandMobileTaskForce] / (float)Player.List.Count() * 100f);
                    int ciVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandChaosInsurgency] / (float)Player.List.Count() * 100f);
                    if (mtfVotePercent >= Callvote.Instance.Config.ThresholdRespawnWave)
                    {
                        Map.Broadcast(5, Callvote.Instance.Translation.MtfRespawn
                            .Replace("%VotePercent%", mtfVotePercent + "%"));
                        WaveManager.Spawn(WaveManager.Waves[0]);
                    }
                    else if (ciVotePercent >= Callvote.Instance.Config.ThresholdRespawnWave)
                    {
                        Map.Broadcast(5, Callvote.Instance.Translation.CiRespawn
                            .Replace("%VotePercent%", ciVotePercent.ToString()));
                        WaveManager.Spawn(WaveManager.Waves[1]);
                    }
                    else
                    {
                        Map.Broadcast(5, Callvote.Instance.Translation.NoSuccessFullRespawn
                            .Replace("%VotePercent%", noVotePercent.ToString())
                            .Replace("%ThresholdRespawnWave%", Callvote.Instance.Config.ThresholdRespawnWave.ToString()));
                    }
                });
            response = VotingAPI.CurrentVoting.Response;
            return true;
        }
    }
}