using System.Text;
using System.Linq;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using UserSettings.ServerSpecific;

namespace Callvote
{
    public class SettingHandlers
    {
        public static ServerSpecificSettingBase[] CallvoteMenu() => SettingsHelper.GetSettings();
    }
    public class SettingsHelper
    {
        public static ServerSpecificSettingBase[] GetSettings()
        {
            List<ServerSpecificSettingBase> settings = new List<ServerSpecificSettingBase>();
            settings.Add(new SSGroupHeader("Callvote"));
            settings.Add(new SSKeybindSetting(501, $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionYes}!", UnityEngine.KeyCode.Z, hint: $"{Plugin.Instance.Translation.KeybindHint} {Plugin.Instance.Translation.OptionYes}!"));
            settings.Add(new SSKeybindSetting(502, $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionNo}!", UnityEngine.KeyCode.X, hint: $"{Plugin.Instance.Translation.KeybindHint} {Plugin.Instance.Translation.OptionNo}!" ));
            if (Plugin.Instance.Config.EnableRespawnWave) 
            {
                settings.Add(new SSKeybindSetting(503, $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionMtf}!", UnityEngine.KeyCode.C, hint: $"{Plugin.Instance.Translation.KeybindHint} {Plugin.Instance.Translation.OptionMtf}!"));
                settings.Add(new SSKeybindSetting(504, $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionCi}!", UnityEngine.KeyCode.V, hint: $"{Plugin.Instance.Translation.KeybindHint} {Plugin.Instance.Translation.OptionCi}!"));
            }
            return settings.ToArray();
        }
    }
}