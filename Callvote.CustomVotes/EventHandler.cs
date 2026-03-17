using Callvote.API.Features.Votes;
using Callvote.CustomVotes.Features.PredefinedVotes;
using UserSettings.ServerSpecific;

namespace Callvote.CustomVotes
{
    internal class EventHandler
    {
        internal EventHandler()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += this.OnUserInput;
        }

        ~EventHandler()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= this.OnUserInput;
        }

        private void OnUserInput(ReferenceHub sender, ServerSpecificSettingBase settingBase)
        {
            if (!VoteHandler.IsVoteActive || !Plugin.Instance.Config.EnableSsKeybinds || VoteHandler.CurrentVote is not RespawnWaveVote)
            {
                return;
            }

            if (settingBase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                switch (keybindSetting.SettingId)
                {
                    case int id when id == Plugin.Instance.Config.MtfKeybindSettingId:
                        if (VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(Plugin.Instance.Translation.CommandMobileTaskForce, out VoteOption mtfVote))
                        {
                            VoteHandler.CurrentVote.SubmitVoteOption(sender, mtfVote);
                        }

                        break;

                    case int id when id == Plugin.Instance.Config.CiKeybindSettingId:
                        if (VoteHandler.CurrentVote.TryGetVoteOptionFromCommand(Plugin.Instance.Translation.CommandChaosInsurgency, out VoteOption ciVote))
                        {
                            VoteHandler.CurrentVote.SubmitVoteOption(sender, ciVote);
                        }

                        break;
                }
            }
        }
    }
}
