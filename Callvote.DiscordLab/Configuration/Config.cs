using System.ComponentModel;

namespace Callvote.DiscordLab.Configuration
{
    public class Config
    {
        [Description("If DiscordLab is present and this is set to a valid Channel Id, sends the vote results there. 0 = disabled")]
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
