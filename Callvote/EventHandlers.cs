using Callvote.API;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using HarmonyLib;
using UserSettings.ServerSpecific;

namespace Callvote
{
    public class EventHandlers
    {
        public void OnWaitingForPlayers()
        {
            VotingHandler.Clear();
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            VotingHandler.Clear();
        }

        public void OnRoundRestarting()
        {
            VotingHandler.Clear();
        }

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
        }
    }
}
