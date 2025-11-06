using System.ComponentModel;

namespace Callvote.Configuration
{
    public class Translation
    {
        [Description("%player%, %VotePercent%, %Offender%, %ThresholdKick%, %ThresholdRespawnWave%, %ThresholdNuke%, %ThresholdKill%, %ThresholdRestartRound%, %OptionKey%, %Option%, %Counter%, %Timer%, %Custom%, %Type%, %Number%")]
        public string AskedQuestion { get; set; } = "%Question% \n <color=#bce3a3>Press ~ and type</color> or <color=#939383>Set the keybind in Server-specific!</color>\n";
        public string Results { get; set; } = "Final results:\n";
        public string OptionAndCounter { get; set; } = " %Option% (%Counter%) ";
        [Description("Options:")]
        public string OptionYes { get; set; } = "<color=green>YES</color>";
        public string OptionNo { get; set; } = "<color=red>NO</color>";
        public string OptionMtf { get; set; } = "<color=blue>MTF</color>";
        public string OptionCi { get; set; } = "<color=green>CI</color>";
        public string Options { get; set; } = ".%OptionKey% = %Option% ";
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
        public string PlayerKilled { get; set; } = "%VotePercent%% voted yes. Killing player %Offender%";
        public string RoundRestarting { get; set; } = "%VotePercent% voted yes. Restarting the round...";
        public string FoundationNuked { get; set; } = "%VotePercent%% voted yes. Nuking the facility...";
        public string CiRespawn { get; set; } = "%VotePercent%% voted <color=green>YES</color>. Forcing the reappearing of CI..";
        public string EnablingFriendlyFire { get; set; } = "%VotePercent%% voted <color=green>YES</color>. Disabling Friendly Fire..";
        public string DisablingFriendlyFire { get; set; } = "%VotePercent%% voted <color=green>YES</color>. Enabling Friendly Fire..";
        public string MtfRespawn { get; set; } = "%VotePercent%% voted <color=green>YES</color>. Forcing the reappearing of MTF..";
        public string PlayerKicked { get; set; } = "%Offender% was kicked for %Reason%.";
        [Description("Vote Failed Messages:")]
        public string NotSuccessFullKick { get; set; } = "%VotePercent%% voted yes, but %ThresholdKick%% was required to kick %Offender%.";
        public string NoSuccessFullRespawn { get; set; } = "%VotePercent%% voted no. %ThresholdRespawnWave%% was required to respawn the next wave.";
        public string NoSuccessFullNuke { get; set; } = "Only %VotePercent%% voted yes. %ThresholdNuke%% was required to nuke the facility.";
        public string NoSuccessFullKill { get; set; } = "Only %VotePercent%% voted yes. %ThresholdKill%% was required to kill %Offender%";
        public string NoSuccessFullRestart { get; set; } = "Only %VotePercent%% voted yes. %ThresholdRestartRound%% was required to restart the round.";
        public string NoSuccessFullEnableFf { get; set; } = "Only %VotePercent%% voted yes. %ThresholdFF%% was required to disable Friendly Fire.";
        public string NoSuccessFullDisableFf { get; set; } = "Only %VotePercent%% voted yes. %ThresholdFF%% was required to enable Friendly Fire.";
        public string Untouchable { get; set; } = "%VotePercent%% voted to kill or kick you.";
        [Description("Console Messages:")]
        public string VotingStarted { get; set; } = "Vote has been started!";
        public string NoVotingInProgress { get; set; } = "There is no vote in progress.";
        public string WaitToVote { get; set; } = "You should wait %Timer%s before using this command.";
        public string PassReason { get; set; } = "You need to pass a reason!";
        public string LessThanTwoOptions { get; set; } = "You cannot create a custom voting without 2 options!";
        public string DuplicateCommand { get; set; } = "It's not possible to create a custom command with the same name!";
        public string QueueDisabled { get; set; } = "Callvote queue disabled.";
        public string QueueCleared { get; set; } = "Votings Queue Cleared.";
        public string QueuePaused { get; set; } = "Queue Paused";
        public string QueueResumed { get; set; } = "Votings Queue resumed.";
        public string RemovedFromQueue { get; set; } = "Removed %Number% Voting(s)";
        public string NoVotingInQueue { get; set; } = "There's no voting in the queue.";
        public string QueueIsFull { get; set; } = "<color=red>Queue is full.</color>";
        public string VotingEnqueued { get; set; } = "<color=#EDF193>Voting Enqueued.</color>";
        public string TypeNotFound { get; set; } = "Did not find any Voting with the type <color=red>%Type%</color>";
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
        public string VotingStoped { get; set; } = "Vote stopped.";
        public string MaxVote { get; set; } = "Max amounts of votes done this round";
        public string VotingInProgress { get; set; } = "A vote is currently in progress.";
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
        public string WebhookTitle { get; set; } = "Voting Results:";
        public string WebhookPlayer { get; set; } = "Player:";
        public string WebhookQuestion { get; set; } = "Question:";
        public string WebhookVotes { get; set; } = "Votes:";
    }
}