#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using Callvote.Commands.ParentCommands;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API;
using Callvote.Features.PredefinedVotes;
using CommandSystem;

namespace Callvote.Commands.CallVoteCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteParentCommand))]
#endif
    public class RespawnWaveCommand : ICommand
    {
        public string Command => "respawnwave";

        public string[] Aliases => ["respawn", "wave"];

        public string Description => "Calls a respawn wave vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!CallvotePlugin.Instance.Config.EnableRespawnWave)
            {
                response = CallvotePlugin.Instance.Translation.VoteRespawnWaveDisabled;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.callvoterespawnwave") && player != null)
#else
            if (!player.HasPermissions("cv.callvoterespawnwave") && player != null)
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitRespawnWave)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitRespawnWave - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitRespawnWave)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitRespawnWave - Round.Duration.TotalSeconds:F0}");
#endif
                return false;
            }

            response = VoteHandler.CallVote(new RespawnWaveVote(player));
            return true;
        }
    }
}