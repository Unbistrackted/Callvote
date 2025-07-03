using Callvote.API;
using Callvote.API.VotingsTemplate;
using CommandSystem;
using LabApi.Features.Wrappers;
using LabApi.Features.Permissions;
using System;
using Callvote.Commands.ParentCommands;

namespace Callvote.Commands.VotingCommands
{
    [CommandHandler(typeof(CallVoteCommand))]
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

            if (!player.HasPermissions("cv.callvoterespawnwave") && player != null)
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitRespawnWave)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRespawnWave - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            VotingHandler.CallVoting(new RespawnWaveVoting(player));
            response = VotingHandler.Response;
            return true;
        }
    }
}