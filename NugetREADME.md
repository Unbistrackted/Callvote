# Callvote

**The sucessor of [callvote](https://github.com/PatPeter/callvote).**

A plugin that allows calling and voting for **Kick**,
 **RestartRound**,  **Kill**,
**RespawnWave**, **FriendlyFire**, or **Custom** Votes using **KEYBINDS** or **CONSOLE COMMANDS** in the same format as the Source Engine (Left 4
Dead
2/Counter-Strike: Global Offensive).

If you want to develop using Callvote, please install the [EXILED Nuget Package](https://www.nuget.org/packages/Callvote.EXILED/) or [LabAPI Nuget Package](https://www.nuget.org/packages/Callvote.LabAPI/)

## Examples
```cs
VotingHandler.AddOptionToVoting("nothing", "<size=100>NOTHING!!!</color>");
VotingHandler.AddOptionToVoting("nothing1", "<size=100>NOTHING1!!!</color>");
VotingHandler.CallVoting(new CustomVoting(player, $"{player.Nickname} asks: Do nothing!!!!!", "NothingBurguerPlugin.DoNothing"));
```
```cs
VotingHandler.CallVoting(new CustomVoting(player, $"{player.Nickname} asks: Enable FF?", "NothingBurguerPlugin.FF", new FFVoting(player)));
```
```cs
Dictionary<string, string> options = new()
{
   ["nothing"] = "<size=100>NOTHING!!!</color>",
   ["nothing1"] = "<size=100>NOTHING2!!!</color>"
};

CustomVoting vote = new CustomVoting(player, $"{player.Nickname} asks: Do nothing!!!!!", "NothingBurguerPlugin.DoNothing", null, options);

VotingHandler.CallVoting(vote);
```
```cs
private void ReviveSCPs(DiedEventArgs ev)
{
    if (ev.Player.IsScp)
    {
        void Callback(Voting voting)
        {
            if (voting is not BinaryVoting binaryVoting)
            {
                return;
            }

            int yesPercentage = voting.GetVotePercentage(binaryVoting.YesVote);
            int noPercentage = voting.GetVotePercentage(binaryVoting.NoVote);

            if (yesPercentage > noPercentage)
            {
                ev.Player.RoleManager.ServerSetRole(ev.TargetOldRole, PlayerRoles.RoleChangeReason.None);
                Map.Broadcast(5, $"{ev.TargetOldRole} respawned.");
                return;
            }

            Map.Broadcast(5, "The Voting Failed.");
        }

        BinaryVoting reviveSCP = new BinaryVoting(Server.Host, $"Revive {ev.TargetOldRole}?", $"NothingBurguerPlugin.Respawn", Callback);
        VotingHandler.CallVoting(reviveSCP);
    }
}
```

## Documentation

> https://unbistrackted.github.io/Callvote/

## Download

This plugin requires [Exiled](https://github.com/ExMod-Team/EXILED/releases/latest) or [LabAPI](https://github.com/northwood-studios/LabAPI/releases/latest).

You can download the latest version of Callvote [here](https://github.com/Unbistrackted/Callvote/releases/latest).

## Soft Depedencies

### Replaces Broadcasts with Hints.
❗️ Do **not** install both — they are incompatible with each other.

- [RueI](https://github.com/pawslee/RueI) — You can download it [here](https://github.com/pawslee/RueI/releases/latest).

- ~~[HintServiceMeow](https://github.com/MeowServer/HintServiceMeow/releases/tag/V5.3) — You can download it [here](https://github.com/MeowServer/HintServiceMeow/releases/latest).~~ **HSM is deprecated.**

## Special thanks to:

https://github.com/Playeroth and https://github.com/Edi369 for helping me with translations and adding webhook functionality.

https://github.com/PatPeter for giving the permission to continue the development of [callvote](https://github.com/PatPeter/callvote).

https://github.com/vladflotsky for giving adivice and guidance while I was rewritting the plugin.
