﻿using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UserSettings.ServerSpecific;

namespace Callvote.Features
{
    public static class ServerSpecificSettings
    {
        public static IEnumerable<ServerSpecificSettingBase> CallvoteSettings { get; private set; }
        public static SSGroupHeader SettingsHeader { get; set; } = new SSGroupHeader(AssemblyInfo.Name);
        public static SSKeybindSetting YesKeybindSetting { get; set; }
        public static SSKeybindSetting NoKeybindSetting { get; set; }
        public static SSKeybindSetting MtfKeybindSetting { get; set; }
        public static SSKeybindSetting CiKeybindSetting { get; set; }
        public static void RegisterSettings()
        {
            YesKeybindSetting = new SSKeybindSetting(888, Callvote.Instance.Translation.VoteKeybind.Replace("%Option%", Callvote.Instance.Translation.OptionYes), KeyCode.Y, hint: Callvote.Instance.Translation.KeybindHint.Replace("%Option%", Callvote.Instance.Translation.OptionYes));
            NoKeybindSetting = new SSKeybindSetting(889, Callvote.Instance.Translation.VoteKeybind.Replace("%Option%", Callvote.Instance.Translation.OptionNo), KeyCode.U, hint: Callvote.Instance.Translation.KeybindHint.Replace("%Option%", Callvote.Instance.Translation.OptionNo));

            if (Callvote.Instance.Config.EnableRespawnWave)
            {
                MtfKeybindSetting = new SSKeybindSetting(890, Callvote.Instance.Translation.VoteKeybind.Replace("%Option%", Callvote.Instance.Translation.OptionMtf), KeyCode.I, hint: Callvote.Instance.Translation.KeybindHint.Replace("%Option%", Callvote.Instance.Translation.OptionMtf));
                CiKeybindSetting = new SSKeybindSetting(891, Callvote.Instance.Translation.VoteKeybind.Replace("%Option%", Callvote.Instance.Translation.OptionCi), KeyCode.O, hint: Callvote.Instance.Translation.KeybindHint.Replace("%Option%", Callvote.Instance.Translation.OptionCi));
            }

            CallvoteSettings =
                [
                SettingsHeader,
                YesKeybindSetting,
                NoKeybindSetting,
                MtfKeybindSetting,
                CiKeybindSetting
                ];

            Register(CallvoteSettings);
        }
        public static void UnregisterSettings()
        {
           Unregister(CallvoteSettings);
        }

        public static void Register(IEnumerable<ServerSpecificSettingBase> settings)
        {
            List<ServerSpecificSettingBase> list = new List<ServerSpecificSettingBase>(ServerSpecificSettingsSync.DefinedSettings ?? Array.Empty<ServerSpecificSettingBase>());

            list.AddRange(settings);

            ServerSpecificSettingsSync.DefinedSettings = list.ToArray();
            ServerSpecificSettingsSync.SendToAll();

        }

        public static void Unregister(IEnumerable<ServerSpecificSettingBase> settings)
        {
            List<ServerSpecificSettingBase> list = new List<ServerSpecificSettingBase>(ServerSpecificSettingsSync.DefinedSettings ?? Array.Empty<ServerSpecificSettingBase>());

            foreach (ServerSpecificSettingBase setting in settings)
                list.Remove(setting);

            ServerSpecificSettingsSync.DefinedSettings = list.ToArray();
            ServerSpecificSettingsSync.SendToAll();
        }
    }
}