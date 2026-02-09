# Callvote

**The sucessor of [callvote](https://github.com/PatPeter/callvote).**

A plugin that allows calling and vote for **Kick**,
 **RestartRound**,  **Kill**,
**RespawnWave**, **FriendlyFire**, or **Custom** Votes using **KEYBINDS** or **CONSOLE COMMANDS** in the same format as the Source Engine (Left 4
Dead
2/Counter-Strike: Global Offensive).

If you want to develop using Callvote, please install the [EXILED Nuget Package](https://www.nuget.org/packages/Callvote.EXILED/) or [LabAPI Nuget Package](https://www.nuget.org/packages/Callvote.LabAPI/)

## Examples
```cs
VoteHandler.CreateVoteOption("command", "<color=red>detail</color>", out _);
VoteHandler.CreateVoteOption("command2", "<color=green>detail2</color>", out _);
VoteHandler.CallVote(new CustomVote(player, $"<color=#D681DE>question</color>", "CallvoteExample.Template"));
```

```cs
VoteHandler.CallVote(new CustomVote(player, $"{player.Nickname} asks: Enable FF?", "CallvoteExample.FF", new FFVote(player)));
```

```cs
private void ReviveSCPs(DiedEventArgs ev)
{
    if (ev.Player.IsScp)
    {
        void Callback(Vote vote)
        {
            if (vote is not BinaryVote binaryVote)
            {
                return;
            }

            int yesPercentage = vote.GetVoteOptionPercentage(binaryVote.YesVoteOption);
            int noPercentage = vote.GetVoteOptionPercentage(binaryVote.NoVoteOption);

            if (yesPercentage > noPercentage)
            {
                ev.Player.RoleManager.ServerSetRole(ev.TargetOldRole, PlayerRoles.RoleChangeReason.None);
                Map.Broadcast(5, $"{ev.TargetOldRole} respawned.");
                return;
            }

            Map.Broadcast(5, "The Vote Failed.");
        }

        BinaryVote reviveSCP = new BinaryVote(Server.Host, $"Revive {ev.TargetOldRole}?", $"NothingBurguerPlugin.Respawn", Callback);
        VoteHandler.CallVote(reviveSCP);
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
