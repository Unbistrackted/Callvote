using Callvote.API;
using CommandSystem;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Wrappers;
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

        public void OnRoundRestarted()
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
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(Callvote.Instance.Translation.CommandYes, out ICommand yesCommand))
                        {
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), yesCommand.Command);
                        }
                        break;
                    case int id when id == 889:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(Callvote.Instance.Translation.CommandNo, out ICommand noCommand))
                        {
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), noCommand.Command);
                        }
                        break;
                    case int id when id == 890:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(Callvote.Instance.Translation.CommandMobileTaskForce, out ICommand mtfCommand))
                        {
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), mtfCommand.Command);
                        }
                        break;
                    case int id when id == 891:
                        if (VotingHandler.CurrentVoting.CommandList.TryGetValue(Callvote.Instance.Translation.CommandChaosInsurgency, out ICommand ciCommand))
                        {
                            VotingHandler.CurrentVoting.Vote(Player.Get(sender), ciCommand.Command);
                        }
                        break;
                }
            }
        }
    }
}
