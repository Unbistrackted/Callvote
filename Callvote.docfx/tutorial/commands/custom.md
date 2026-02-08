---
uid: customcommand
---

# Custom Votings
How to call and customize your ``Custom Voting`` using Commands.

> [!NOTE]
> This guide assumes you know **Callvote's Voting Structure**. Please refer to [Quick Start](xref:quickstart).

> [!IMPORTANT]
> Remove the square brackets when using the command, they only serve as separation in this example.

Custom Votings can be a bit tricky but once you learn how to use them, it becomes a walk in the park.

### How to call

```
callvote custom ["Question"] [Option 1]([Detail 1]) [Option 2]([Detail 2]) [Option 3]([Detail 3]) ...
```

> [!WARNING]
> This Voting requires the ``cv.callvotecustom`` permission.


``Callvote Custom`` needs to follow these rules:

> [!]
> ``Question`` <span style="color:#00FF1D">**MUST**</span> be inside quotes, <span style="color:#00FF1D">**CAN**</span> contain spaces and <span style="color:#00FF1D">**CAN**</span> contain [Rich Tags](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html).
>
> ``Option 1``, ``Option 2`` and ``Option 3`` <span style="color:#FF0000">**MUST NOT**</span> be the same, <span style="color:#FF0000">**CANNOT**</span>  contain spaces and  <span style="color:#FF0000">**CANNOT**</span> contain [Rich Tags](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html).
>
> ``Detail 1``,  ``Detail 2`` and ``Detail 3`` <span style="color:#00FF1D">**MUST**</span> be inside parentheses, <span style="color:#00FF1D">**CAN**</span> contain spaces and <span style="color:#00FF1D">**CAN**</span> contain [Rich Tags](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html).
>
> Each ``Option`` <span style="color:#00FF1D">**MUST**</span> be close to a ``Detail``.

Custom Voting can have as many or as few questions as you want.

So by using this example:

```
callvote custom "<color=#D681DE>question</color>" command(<color=red>detail</color>) command2(<color=green>detail2</color>)
```

You should expect this Voting to pop up:

![alt-text](/images/CallvoteCustom.png)

