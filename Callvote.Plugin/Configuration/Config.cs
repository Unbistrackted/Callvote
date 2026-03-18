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

#endif
        [Description("Are the plugin's debug logs enabled")]
        public bool Debug { get; set; } = false;

        [Description("Which message provider should Callvote use? You can choose between auto, ruei, or broadcasts / bc.")]
        public string MessageProvider { get; set; } = "auto";

        [Description("Sets the Y coordinate of the hint on a scale from 0-1000, where 0 represents the bottom of the screen (Doesn't apply for broadcasts)")]
        public float HintYCoordinate { get; set; } = 300;

        [Description("Enables or disables the Callvote's logo on startup")]
        public bool ShowLogo { get; set; } = true;

        [Description("Changes the vote duration.")]
        public int VoteDuration { get; set; } = 30;

        [Description("Changes Callvote's message size. (0 = Callvote's default size calculation algorithm)")]
        public int MessageSize { get; set; } = 0;

        [Description("Changes message's refresh time.")]
        public float RefreshInterval { get; set; } = 1f;

        [Description("Changes Callvote's results message duration.")]
        public int FinalResultsDuration { get; set; } = 5;

        [Description("Add a Discord Webhook if you want to send a Vote Result message to using discord's webhook.")]
        public string DiscordWebhook { get; set; } = string.Empty;

        [Description("Callvote's SS Menu related settings.")]
        public bool EnableSSMenu { get; set; } = true;

        public int HeaderSettingId { get; set; } = 887;

        public int YesKeybindSettingId { get; set; } = 888;

        public int NoKeybindSettingId { get; set; } = 889;
    }
}