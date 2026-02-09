#if EXILED
using Exiled.API.Interfaces;
#endif
using System.ComponentModel;

namespace Callvote.Configuration
{
#if EXILED
    public class Translation : ITranslation
#else
    public class Translation
#endif
    {
        [Description("%player%, %VotePercent%, %Offender%, %ThresholdKick%, %ThresholdRespawnWave%, %ThresholdNuke%, %ThresholdKill%, %ThresholdRestartRound%, %VoteDetail%, %VoteCommand%, %VoteCounter%, %Timer%, %Custom%, %Type%, %Number%")]
        public string AskedQuestion { get; set; } = "%Question% \n <color=#bce3a3>Press ~ and type</color> or <color=#939383>Set the keybind in Server-specific!</color>\n";

        public string Results { get; set; } = "Final results:\n";

        public string OptionAndCounter { get; set; } = " %VoteDetail% (%VoteCounter%) ";

        public string Options { get; set; } = ".%VoteCommand% = %VoteDetail% ";

        [Description("Details:")]
        public string DetailYes { get; set; } = "<color=green>YES</color>";

        public string DetailNo { get; set; } = "<color=red>NO</color>";

        public string DetailMtf { get; set; } = "<color=blue>MTF</color>";

        public string DetailCi { get; set; } = "<color=green>CI</color>";

        [Description("Questions:")]
        public string AskedCustom { get; set; } = "%Player% <color=#EEDC8A>asks</color>: %Custom%";

        public string AskedToKill { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Kick %Offender% for %Reason%?";

        public string AskedToRespawn { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Respawn the next wave?";

        public string AskedToNuke { get; set; } = "%Player% <color=#EEDC8A>asks</color>: NUKE THE FACILITY?!??";

        public string AskedToDisableFf { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Disable Friendly Fire for the current round?";

        public string AskedToEnableFf { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Enable Friendly Fire for the current round?";

        public string AskedToKick { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Kick %Offender% for %Reason%?";

        public string AskedToRestart { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Restart the round?";

        [Description("Vote Passed Messages:")]
        public string PlayerKilled { get; set; } = "%VotePercent%% voted %VoteDetail%.. Killing player %Offender%";

        public string RoundRestarting { get; set; } = "%VotePercent% voted %VoteDetail%.. Restarting the round...";

        public string FoundationNuked { get; set; } = "%VotePercent%% voted %VoteDetail%.. Nuking the facility...";

        public string CiRespawn { get; set; } = "%VotePercent%% voted %VoteDetail%. Forcing the reappearing of CI..";

        public string EnablingFriendlyFire { get; set; } = "%VotePercent%% voted %VoteDetail%. Enabling Friendly Fire..";

        public string DisablingFriendlyFire { get; set; } = "%VotePercent%% voted %VoteDetail%. Disabling Friendly Fire..";

        public string MtfRespawn { get; set; } = "%VotePercent%% voted %VoteDetail%. Forcing the reappearing of MTF..";

        public string PlayerKicked { get; set; } = "%Offender% was kicked for %Reason%.";

        [Description("Vote Failed Messages:")]
        public string NotSuccessFullKick { get; set; } = "%VotePercent%% voted %VoteDetail%, but %ThresholdKick%% was required to kick %Offender%.";

        public string NoSuccessFullRespawn { get; set; } = "%VotePercent%% voted %VoteDetail%. %ThresholdRespawnWave%% was required to respawn the next wave.";

        public string NoSuccessFullNuke { get; set; } = "Only %VotePercent%% voted %VoteDetail%. %ThresholdNuke%% was required to nuke the facility.";

        public string NoSuccessFullKill { get; set; } = "Only %VotePercent%% voted %VoteDetail%. %ThresholdKill%% was required to kill %Offender%";

        public string NoSuccessFullRestart { get; set; } = "Only %VotePercent%% voted %VoteDetail%. %ThresholdRestartRound%% was required to restart the round.";

        public string NoSuccessFullEnableFf { get; set; } = "Only %VotePercent%% voted %VoteDetail%. %ThresholdFF%% was required to disable Friendly Fire.";

        public string NoSuccessFullDisableFf { get; set; } = "Only %VotePercent%% voted %VoteDetail%. %ThresholdFF%% was required to enable Friendly Fire.";

        public string Untouchable { get; set; } = "%VotePercent%% voted to kill or kick you.";

        [Description("Console Messages:")]
        public string VoteStarted { get; set; } = "Vote has been started!";

        public string NoVoteInProgress { get; set; } = "There is no vote in progress.";

        public string WaitToVote { get; set; } = "You should wait %Timer%s before using this command.";

        public string PassReason { get; set; } = "You need to pass a reason!";

        public string LessThanTwoOptions { get; set; } = "You cannot create a custom vote without 2 options!";

        public string DuplicateCommand { get; set; } = "It's not possible to create a custom command with the same name!";

        public string QueueDisabled { get; set; } = "Callvote queue disabled.";

        public string QueueCleared { get; set; } = "Vote Queue Cleared.";

        public string QueuePaused { get; set; } = "Queue Paused";

        public string QueueResumed { get; set; } = "Vote Queue resumed.";

        public string RemovedFromQueue { get; set; } = "Removed %Number% Vote(s)";

        public string NoVoteInQueue { get; set; } = "There's no vote in the queue.";

        public string QueueIsFull { get; set; } = "<color=red>Queue is full.</color>";

        public string VoteEnqueued { get; set; } = "<color=#EDF193>Vote Enqueued.</color>";

        public string TypeNotFound { get; set; } = "Did not find any Vote with the type <color=red>%Type%</color>";

        public string InvalidArgument { get; set; } = "Invalid argument.";

        public string TranslationChanged { get; set; } = "Changed translation to: English";

        public string VoteRespawnWaveDisabled { get; set; } = "Callvote RespawnWave is disabled.";

        public string VoteKickDisabled { get; set; } = "Callvote kick is disabled.";

        public string VoteKillDisabled { get; set; } = "Callvote kill is disabled.";

        public string VoteNukeDisabled { get; set; } = "Callvote nuke is disabled.";

        public string VoteFFDisabled { get; set; } = "Callvote ff is disabled.";

        public string VoteRestartRoundDisabled { get; set; } = "Callvote restartround is disabled.";

        public string PlayerNotFound { get; set; } = "Did not find any players with the name or partial name of %Player%";

        public string NoOptionAvailable { get; set; } = "Vote does not have the option %Option%.";

        public string AlreadyVoted { get; set; } = "You've already voted.";

        public string VoteAccepted { get; set; } = "You voted %Option%.";

        public string NoPermission { get; set; } = "You do not have permission to run this command!";

        public string VoteStoped { get; set; } = "Vote stopped.";

        public string MaxVote { get; set; } = "Max amounts of votes done this round";

        public string VoteInProgress { get; set; } = "A vote is currently in progress.";

        public string PlayersWithSameName { get; set; } = "Multiple players have a name or partial name of %Player%. Please use a different search string.";

        public string WrongSyntax { get; set; } = "Wrong Syntax, use ~callvote help~";

        [Description("Keybinds:")]
        public string VoteKeybind { get; set; } = "Vote %Option%!";

        public string KeybindHint { get; set; } = "Set this keybind to vote %Option%.";

        [Description("Commands:")]
        public string CommandYes { get; set; } = "yes";

        public string CommandNo { get; set; } = "no";

        public string CommandChaosInsurgency { get; set; } = "ci";

        public string CommandMobileTaskForce { get; set; } = "mtf";

        [Description("Webhook:")]
        public string WebhookTitle { get; set; } = "Vote Results:";

        public string WebhookPlayer { get; set; } = "Player:";

        public string WebhookQuestion { get; set; } = "Question:";

        public string WebhookVotes { get; set; } = "Votes:";
    }
}