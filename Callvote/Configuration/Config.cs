#if EXILED
using Exiled.API.Interfaces;
#endif
using System.ComponentModel;

namespace Callvote.Configuration
{
#if EXILED
    public class Config : IConfig
#else
    public class Config
#endif
    {
#if EXILED
        [Description("Is the plugin enabled")]
        public bool IsEnabled { get; set; } = true;

        [Description("Are the plugin's debug logs enabled")]
        public bool Debug { get; set; } = false;

#endif
        [Description("Which message provider should Callvote use? You can choose between auto, hsm, ruei, or broadcasts / bc. (In auto mode, if both HSM and RUEI are present on the server, it falls back to broadcasts.)")]
        public string MessageProvider { get; set; } = "auto";

        [Description("Sets the Y coordinate of the hint on a scale from 0-1000, where 0 represents the bottom of the screen (Doesn't apply for broadcasts)")]
        public float HintYCoordinate { get; set; } = 300;

        [Description("Enable or disable Modules.")]
        public bool EnableQueue { get; set; } = true;

        public bool EnableKick { get; set; } = true;

        public bool EnableFf { get; set; } = true;

        public bool EnableKill { get; set; } = true;

        public bool EnableNuke { get; set; } = true;

        public bool EnableRespawnWave { get; set; } = true;

        public bool EnableRoundRestart { get; set; } = true;

        [Description("Changes the voting duration.")]
        public int VoteDuration { get; set; } = 30;

        [Description("Changes the maximum amount of voting each player can call in a match.")]
        public int MaxAmountOfVotesPerRound { get; set; } = 10;

        [Description("Changes the amount of time it needs to start voting after the round starts for each module.")]
        public float MaxWaitFf { get; set; } = 0;

        public float MaxWaitKill { get; set; } = 0;

        public float MaxWaitKick { get; set; } = 0;

        public float MaxWaitNuke { get; set; } = 0;

        public float MaxWaitRespawnWave { get; set; } = 0;

        public float MaxWaitRestartRound { get; set; } = 0;

        [Description("Changes the threshold to pass the voting for each module.")]
        public int ThresholdKick { get; set; } = 30;

        public int ThresholdFf { get; set; } = 30;

        public int ThresholdKill { get; set; } = 30;

        public int ThresholdNuke { get; set; } = 30;

        public int ThresholdRespawnWave { get; set; } = 30;

        public int ThresholdRestartRound { get; set; } = 30;

        [Description("Changes Callvote's message size. (0 = Callvote's default size calculation algorithm)")]
        public int MessageSize { get; set; } = 0;

        [Description("Changes message's refresh time.")]
        public float RefreshInterval { get; set; } = 1f;

        [Description("Changes Callvote's results message duration.")]
        public int FinalResultsDuration { get; set; } = 5;

        [Description("Changes Callvote's Queue size if enabled.")]
        public int QueueSize { get; set; } = 5;

        [Description("Add a Discord Webhook if you want to send a Voting Result message to discord.")]
        public string DiscordWebhook { get; set; } = string.Empty;

        [Description("Changes Callvote's SS Menu settings ID.")]
        public int HeaderSettingId { get; set; } = 887;

        public int YesKeybindSettingId { get; set; } = 888;

        public int NoKeybindSettingId { get; set; } = 889;

        public int MtfKeybindSettingId { get; set; } = 890;

        public int CiKeybindSettingId { get; set; } = 891;
    }
}