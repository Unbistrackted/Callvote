using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace Callvote.Commands
{
    public class BinaryCommand : ICommand
    {
        public string Command => "binary";

        public string[] Aliases => new[] { "binario", "bi", "b" };

        public string Description => "Calls a binary voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {

            Player player = Player.Get(sender);

            if (!player.CheckPermission("cv.callvotecustom") || !player.CheckPermission("cv.bypass"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return true;
            }

            CallvoteAPI.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            CallvoteAPI.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);

            CallvoteAPI.CallVoting(
                Callvote.Instance.Translation.AskedCustom
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Custom%", string.Join(" ", args)),
                nameof(Enums.VotingType.Binary),
                player,
                null);
            response = CallvoteAPI.Response;
            return true;
        }
    }
}