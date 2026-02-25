using Callvote.API;
using Callvote.API.Votes;
using Callvote.Configuration;
using UserSettings.ServerSpecific;

namespace Callvote
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only public API documentation is required")]
    internal class EventHandlers
    {
        internal EventHandlers()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += this.OnUserInput;
        }

        ~EventHandlers()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= this.OnUserInput;
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

                    case int id when id == CallvotePlugin.Instance.Config.MtfKeybindSettingId:
                        if (VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(CallvotePlugin.Instance.Translation.CommandMobileTaskForce, out VoteOption mtfVote))
                        {
                            VoteHandler.CurrentVote.SubmitVoteOption(sender, mtfVote);
                        }

                        break;

                    case int id when id == CallvotePlugin.Instance.Config.CiKeybindSettingId:
                        if (VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(CallvotePlugin.Instance.Translation.CommandChaosInsurgency, out VoteOption ciVote))
                        {
                            VoteHandler.CurrentVote.SubmitVoteOption(sender, ciVote);
                        }

                        break;
                }
            }
        }
    }
}