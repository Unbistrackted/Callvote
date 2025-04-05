using Callvote.API;
using HarmonyLib;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Events.Handlers;

namespace Callvote
{
    public class EventHandlers : CustomEventsHandler
    {
        public override void OnServerWaitingForPlayers()
        {
            ClearVotings();
        }

        public override void OnServerRoundEnded(RoundEndedEventArgs ev)
        {
            ClearVotings();
        }

        public override void OnServerRoundRestarted()
        {
            ClearVotings();
        }
        /*
        public void OnUserInput(ReferenceHub sender, ServerSpecificSettingBase settingBase)
        {
            if (VotingHandler.CurrentVoting == null)
                return;
            if (settingBase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                switch (keybindSetting.SettingId)
                {
                    case int id when id == 888:
                        VotingHandler.CurrentVoting.Vote(Player.Get(sender), VotingHandler.CurrentVoting.CommandList.GetValueSafe(Callvote.Instance.Translation.OptionYes).Command);
                        break;
                    case int id when id == 889:
                        VotingHandler.CurrentVoting.Vote(Player.Get(sender), VotingHandler.CurrentVoting.CommandList.GetValueSafe(Callvote.Instance.Translation.OptionNo).Command);
                        break;
                    case int id when id == 890:
                        VotingHandler.CurrentVoting.Vote(Player.Get(sender), VotingHandler.CurrentVoting.CommandList.GetValueSafe(Callvote.Instance.Translation.OptionMtf).Command);
                        break;
                    case int id when id == 891:
                        VotingHandler.CurrentVoting.Vote(Player.Get(sender), VotingHandler.CurrentVoting.CommandList.GetValueSafe(Callvote.Instance.Translation.OptionCi).Command);
                        break;
                }
            }
        } */

        private static void ClearVotings()
        {
            if (Callvote.Instance.Config.EnableQueue) { VotingHandler.VotingQueue.Clear(); }
            VotingHandler.FinishVoting();
        }
    }
}
