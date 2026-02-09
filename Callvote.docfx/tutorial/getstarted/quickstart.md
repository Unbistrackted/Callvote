---
uid: quickstart
---

# Quick Start
Quick guide if you don't want to spend your time reading the entire documentation.

> [!NOTE]
> The terms Option and Command in the Vote context can be interchangeable.

### Vote Structure

A Vote can have the following elements:

![alt-text](~/images/CallvoteSchematic.png)

And each one can be controlled by the user in these given ways:

# [Command Users](#tab/Command)

> [!WARNING]
> The Vote will not start if you register 2 or more Vote Options with the same name!

You can reproduce the same Vote using this Command:

```
callvote custom "<color=#D681DE>question</color>" command(<color=red>detail</color>) command2(<color=green>detail2</color>)
```

Or create a Yes or No Vote with:

```
callvote binary "<color=#D681DE>question</color>"
```

There's also the Predefined Votes, which you look for by typing the help command or looking at this page: (xref:othercommand)

# [Developers](#tab/Devs)

> [!WARNING]
> The Vote will not start if you register 2 or more Vote Options with the same name!

You can reproduce the same Vote using this snipet:

```cs
VoteHandler.CreateVoteOption("command", "<color=red>detail</color>", out _);
VoteHandler.CreateVoteOption("command2", "<color=green>detail2</color>", out _);
VoteHandler.CallVote(new CustomVote(player, $"<color=#D681DE>question</color>", "CallvoteExample.Template"));
```

Or create a Custom Vote using the Predefined ones:

```cs
VoteHandler.CallVote(new CustomVote(player, $"{player.Nickname} asks: Enable FF?", "CallvoteExample.FF", new FFVote(player)));
```

You can also pass the @Callvote.API.VoteTemplate.CustomVote constructor a <xref:Callvote.Features.Vote.Callback> and make your own behaviour, like this:

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

<xref:Callvote.API.VoteHandler.CallVote(Callvote.Features.Vote)> returns the vote status as a string, subject to change to an enum in the future.




