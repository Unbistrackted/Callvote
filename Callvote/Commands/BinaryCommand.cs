using System;
using System.Collections.Generic;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    public class BinaryCommand : ICommand
    {
        public string Command => "binary";

        public string[] Aliases => new[] { "binario", "bi", "b" };

        public string Description => "Calls a binary voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get(sender);

            if (!player.CheckPermission("cv.callvotecustom") || !player.CheckPermission("cv.bypass"))
            {
                response = Callvote.Instance.Translation.NoPermissionToVote;
                return true;
            }

            options.Add(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            options.Add(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);

            VotingAPI.CurrentVoting = new Voting(Callvote.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname).Replace("%Custom%", string.Join(" ", args)), options, player, null);
            response = VotingAPI.CurrentVoting.Response;
            return true;
        }
    }
}