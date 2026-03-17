using System.ComponentModel;

namespace Callvote.ScpDiscord.Configuration
{
    public class Config
    {
        [Description("If SCPDiscord is present and this is set to a valid Channel Id, sends the vote results there. 0 = disabled")]
        public ulong DiscordChannelId { get; set; } = 0;

        [Description("Embed:")]
        public string EmbedTitle { get; set; } = "Vote Results:";

        public string EmbedPlayer { get; set; } = "Player:";

        public string EmbedQuestion { get; set; } = "Question:";

        public string EmbedVotes { get; set; } = "Votes:";
    }
}
