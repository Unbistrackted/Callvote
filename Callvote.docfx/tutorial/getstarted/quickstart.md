---
uid: quickstart
---

# Quick Start
Quick guide if you don't want to spend your time reading the entire documentation.

> [!NOTE]
> The terms Option and Command in the Voting context can be interchangeable.

### Voting Structure

A Voting can have the following elements:

![alt-text](/images/CallvoteSchematic.png)

And each one can be controlled by the user in these given ways:

# [Command Users](#tab/Command)

> [!WARNING]
> The Voting will not start if you register 2 or more options with the same name!

You can reproduce the same Voting using this Command:

```
callvote custom "<color=#D681DE>question</color>" command(<color=red>detail</color>) command2(<color=green>detail2</color>)
```

Or create a Yes or No Voting with:

```
callvote binary "<color=#D681DE>question</color>"
```

There's also the Predefined Votings, which you look for by typing the help command:

```
cv help
```

# [Developers](#tab/Devs)

> [!WARNING]
> The Voting will not start if you register 2 or more options with the same name!

You can reproduce the same Voting using this snipet:

```cs
VotingHandler.AddOptionToVoting("command", "<color=red>detail</color>");
VotingHandler.AddOptionToVoting("command2", "<color=green>detail2</color>");
VotingHandler.CallVoting(new CustomVoting(player, $"<color=#D681DE>question</color>", "CallvoteExample.Template"));
```

Or create a Custom Voting using the Predefined ones:

```cs
VotingHandler.CallVoting(new CustomVoting(player, $"{player.Nickname} asks: Enable FF?", "CallvoteExample.FF", new FFVoting(player)));
```

You can also pass the @Callvote.API.VotingsTemplate.CustomVoting constructor a <xref:Callvote.Features.Voting.Callback> and make your own behaviour based on the <xref:Callvote.Features.Voting.Counter>, like this:

```cs

private void ReviveSCPs(DiedEventArgs ev)
{
   if (ev.Player.IsScp)
      {
         void callback(Voting vote)
         {
            int yes = vote.Counter[Callvote.CallvotePlugin.Instance.Translation.CommandYes];
            int no = vote.Counter[Callvote.CallvotePlugin.Instance.Translation.CommandNo];
            if (yes > no)
            {
               ev.Player.RoleManager.ServerSetRole(ev.TargetOldRole, PlayerRoles.RoleChangeReason.None);
               Map.Broadcast(5, $"{ev.TargetOldRole} respawned.");
               return;
            }
                    Map.Broadcast(5, "The Voting Failed.");
         }
         BinaryVoting reviveSCP = new BinaryVoting(Server.Host, $"Revive {ev.TargetOldRole}?", $"NothingBurguerPlugin.Respawn", callback);
         VotingHandler.CallVoting(reviveSCP);
      }
}
```
<xref:Callvote.API.VotingHandler.CallVoting(Callvote.Features.Voting)> returns the voting status as a string, subject to change to an enum in the future.




