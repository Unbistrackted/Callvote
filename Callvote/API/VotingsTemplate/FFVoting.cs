﻿using Callvote.Enums;
using Callvote.Features;
using Callvote.Interfaces;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.API.VotingsTemplate
{
    public class FFVoting : Voting, IVotingTemplate
    {
        public FFVoting(Player player) : base(
            ReplacePlayer(player),
            nameof(VotingTypeEnum.Ff),
            player,
            AddCallback,
            AddOptions())
        {
        }

        public static void AddCallback(Voting vote)
        {
            int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
            if (yesVotePercent >= Callvote.Instance.Config.ThresholdFf && yesVotePercent > noVotePercent)
            {
                Server.FriendlyFire = !Server.FriendlyFire;
                string msg = Server.FriendlyFire
                    ? Callvote.Instance.Translation.DisablingFriendlyFire
                    : Callvote.Instance.Translation.EnablingFriendlyFire;
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(msg)}>{msg.Replace("%VotePercent%", yesVotePercent.ToString())}</size>");
            }
            else
            {
                string msg = Server.FriendlyFire
                    ? Callvote.Instance.Translation.NoSuccessFullEnableFf
                    : Callvote.Instance.Translation.NoSuccessFullDisableFf;
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(msg)}>{msg
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdFF%", Callvote.Instance.Config.ThresholdFf.ToString())}</size>");
            }
        }

        public static Dictionary<string, string> AddOptions()
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            return VotingHandler.Options;
        }

        private static string ReplacePlayer(Player player)
        {
            string baseQuestion = Server.FriendlyFire
                ? Callvote.Instance.Translation.AskedToDisableFf
                : Callvote.Instance.Translation.AskedToEnableFf;

            return baseQuestion.Replace("%Player%", player.Nickname);
        }
    }
}
