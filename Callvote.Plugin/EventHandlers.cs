using Callvote.API.Events.EventArgs;
using Callvote.API.Features.Votes;
using Callvote.Features;
using UserSettings.ServerSpecific;

namespace Callvote
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only public API documentation is required")]
    internal class EventHandlers
    {
        internal EventHandlers()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += this.OnUserInput;
            ServerSpecificSettingsSync.SendOnJoinFilter += this.OnSendOnJoinFilter;
            API.Events.EventsHandlers.VoteEnded += this.OnVoteEnded;
        }

        ~EventHandlers()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= this.OnUserInput;
            ServerSpecificSettingsSync.SendOnJoinFilter -= this.OnSendOnJoinFilter;
            API.Events.EventsHandlers.VoteEnded -= this.OnVoteEnded;
        }

        private bool OnSendOnJoinFilter(ReferenceHub hub)
        {
            return true;
        }

        private void OnVoteEnded(VoteEndedEventArgs ev)
        {
            WebhookSender.SendVoteResults(ev.Vote);
        }

        private void OnUserInput(ReferenceHub sender, ServerSpecificSettingBase settingBase)
        {
            if (!VoteHandler.IsVoteActive || !CallvotePlugin.Instance.Config.EnableSSMenu)
            {
                return;
            }

            if (settingBase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                switch (keybindSetting.SettingId)
                {
                    case int id when id == CallvotePlugin.Instance.Config.YesKeybindSettingId:
                        if (VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(CallvotePlugin.Instance.Translation.CommandYes, out VoteOption yesVote))
                        {
                            VoteHandler.CurrentVote.SubmitVoteOption(sender, yesVote);
                        }

                        break;

                    case int id when id == CallvotePlugin.Instance.Config.NoKeybindSettingId:
                        if (VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(CallvotePlugin.Instance.Translation.CommandNo, out VoteOption noVote))
                        {
                            VoteHandler.CurrentVote.SubmitVoteOption(sender, noVote);
                        }

                        break;
                }
            }
        }
    }
}