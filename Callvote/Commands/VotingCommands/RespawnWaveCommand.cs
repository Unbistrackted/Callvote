using Callvote.API;
using Callvote.API.VotingsTemplate;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

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

            if (!player.CheckPermission("cv.callvoterespawnwave") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitRespawnWave)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRespawnWave - Round.ElapsedTime.TotalSeconds:F0}");
                return false;
            }

            VotingHandler.CallVoting(new RespawnWaveVoting(player));
            response = VotingHandler.Response;
            return true;
        }
    }
}