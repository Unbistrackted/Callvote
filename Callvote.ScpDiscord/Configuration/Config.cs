#if EXILED
using Exiled.API.Interfaces;
#endif
using System.ComponentModel;

namespace Callvote.ScpDiscord.Configuration
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

        [Description("If SCPDiscord is present and this is set to a valid Channel Id, sends the vote results there. 0 = disabled")]
        public ulong DiscordChannelId { get; set; } = 0;

        [Description("Change this if you want to use your own embed JSON. %player%, %question%, %winningVoteOption%, %winningVoteOptionColor%")]
        public string EmbedJson { get; set; } = string.Empty;

        [Description("Embed:")]
        public string EmbedTitle { get; set; } = "Vote Results:";

        public string EmbedPlayer { get; set; } = "Player:";

        public string EmbedQuestion { get; set; } = "Question:";

        public string EmbedVotes { get; set; } = "Votes:";
    }
}
