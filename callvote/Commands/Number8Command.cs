﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using UnityEngine;

namespace callvote.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	class Number8Command : ICommand
	{
		public string Command => "8";

		public string[] Aliases => new string[] { };

		public string Description => "";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = "";
			Player player = Player.Get(((CommandSender)sender).SenderId);
			if (sender is PlayerCommandSender)
			{
				var plr = sender as PlayerCommandSender;
				if (Plugin.Instance.Voting())
				{
					response = Plugin.Instance.VoteHandler(player, 8);
				}
				else
				{
					response = "No vote is in progress.";
				}
			}
			return false;
		}
	}
}
