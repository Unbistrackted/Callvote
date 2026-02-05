#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using Callvote.Commands.ParentCommands;
#endif
using Callvote.API;
using Callvote.Features.PredefinedVotings;
using CommandSystem;
using System;

namespace Callvote.Commands.VotingCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteCommand))]
#endif
    public class RespawnWaveCommand : ICommand
    {
        public string Command => "respawnwave";

        public string[] Aliases => ["respawn", "wave"];

        public string Description => "Calls a respawn wave voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableRespawnWave)
            {
                response = Callvote.Instance.Translation.VoteRespawnWaveDisabled;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.callvoterespawnwave") && player != null)
#else
            if (!player.HasPermissions("cv.callvoterespawnwave") && player != null)
#endif
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitRespawnWave)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRespawnWave - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitRespawnWave)
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRespawnWave - Round.Duration.TotalSeconds:F0}");
#endif
                return false;
            }

            VotingHandler.CallVoting(new RespawnWaveVoting(player));
            response = VotingHandler.Response;
            return true;
        }
    }
}