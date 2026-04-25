using System;
using Callvote.API.Enums;
using Callvote.API.Features.Votes;
using Callvote.Commands.ParentCommands;
using Callvote.CustomVotes.Features.PredefinedVotes;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;

namespace Callvote.CustomVotes.Commands
{
    public class RespawnWaveCommand : ICommand
    {
        public string Command => "respawnwave";

        public string[] Aliases => ["respawn", "wave"];

        public string Description => "Calls a respawn wave vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!CustomVotePlugin.Instance.Config.EnableRespawnWave)
            {
                response = CustomVotePlugin.Instance.Translation.VoteRespawnWaveDisabled;
                return false;
            }

            if ((player != null && !player.HasPermissions("cv.callvoterespawnwave")) || (player == null && sender is not ServerConsoleSender))
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (player != null && !player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CustomVotePlugin.Instance.Config.MaxWaitRespawnWave)
            {
                response = CustomVotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CustomVotePlugin.Instance.Config.MaxWaitRespawnWave - Round.Duration.TotalSeconds:F0}");
                return false;
            }

            CallVoteStatus status = VoteHandler.CallVote(new RespawnWaveVote(player ?? Server.Host));

            response = VoteHandler.CurrentVote.GetMessageFromCallVoteStatus(status);
            return true;
        }
    }
}