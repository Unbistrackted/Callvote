using System.Collections.Generic;
using Callvote.Commands;
using Callvote.VoteHandlers;
using Exiled.API.Features;
using GameCore;
using MEC;
using RemoteAdmin;

namespace Callvote.VoteHandlers
{
    public class VoteAPI
    {
        public static Vote CurrentVote = null;
        public static Dictionary<int, int> DictionaryOfVotes = new Dictionary<int, int>();
        public static CoroutineHandle VoteCoroutine;

        public static void NullifyVariables()
        {
            DictionaryOfVotes = null;
            CurrentVote = null;
        }


        public static string Voting(Player player, string option)
        {

            var playerUserId = player.UserId;
            if (VoteAPI.CurrentVote == null) return Plugin.Instance.Translation.NoCallVoteInProgress;
            if (!VoteAPI.CurrentVote.Options.ContainsKey(option))
                return Plugin.Instance.Translation.NoOptionAvailable;
            if (VoteAPI.CurrentVote.Votes.ContainsKey(playerUserId))
            {
                if (VoteAPI.CurrentVote.Votes[playerUserId] == option)
                    return Plugin.Instance.Translation.AlreadyVoted;
                VoteAPI.CurrentVote.Counter[VoteAPI.CurrentVote.Votes[playerUserId]]--;
                VoteAPI.CurrentVote.Votes[playerUserId] = option;
            }

            if (!VoteAPI.CurrentVote.Votes.ContainsKey(playerUserId))
                VoteAPI.CurrentVote.Votes.Add(playerUserId, option);

            VoteAPI.CurrentVote.Counter[option]++;

            return Plugin.Instance.Translation.VoteAccepted.Replace("%Reason%",
                VoteAPI.CurrentVote.Options[option]);
        }


        public static void StartVote(string question, Dictionary<string, string> options, CallvoteFunction callback)
        {
            var newVote = new Vote(question, options);
            VoteAPI.VoteCoroutine = Timing.RunCoroutine(StartVoteCoroutine(newVote, callback));
            foreach (var kvp in options)
            {
                string[] a = { kvp.Key };
                var voteCommand = new VoteCommand(kvp.Key, a);
                QueryProcessor.DotCommandHandler.RegisterCommand(voteCommand);
            }
        }

        public static string StopVote()
        {
            if (!VoteAPI.VoteCoroutine.IsRunning) return Plugin.Instance.Translation.NoCallVoteInProgress;
            Timing.KillCoroutines(VoteAPI.VoteCoroutine);
            foreach (var kvp in VoteAPI.CurrentVote.Options)
            {
                string[] a = { kvp.Key };
                var voteCommand = new VoteCommand(kvp.Key, a);
                QueryProcessor.DotCommandHandler.UnregisterCommand(voteCommand);
            }

            VoteAPI.CurrentVote = null;
            return Plugin.Instance.Translation.CallVoteEnded;
        }


        public static IEnumerator<float> StartVoteCoroutine(Vote newVote, CallvoteFunction callback)
        {
            var timerCounter = 0;
            VoteAPI.CurrentVote = newVote;
            VoteAPI.CurrentVote.Callback = callback;
            var firstBroadcast =
                Plugin.Instance.Translation.AskedQuestion.Replace("%Question%", VoteAPI.CurrentVote.Question);
            var counter = 0;
            foreach (var kv in VoteAPI.CurrentVote.Options)
            {
                if (counter == VoteAPI.CurrentVote.Options.Count - 1)
                    firstBroadcast +=
                        $", {Plugin.Instance.Translation.Options.Replace("%OptionKey%", kv.Key).Replace("%Option%", kv.Value)}";
                else
                    firstBroadcast +=
                        $" {Plugin.Instance.Translation.Options.Replace("%OptionKey%", kv.Key).Replace("%Option%", kv.Value)}";
                counter++;
            }

            var textsize = firstBroadcast.Length / 10;
            Map.Broadcast(5, "<size=" + (48 - textsize) + ">" + firstBroadcast + "</size>");
            yield return Timing.WaitForSeconds(5f);
            for (;;)
            {
                if (timerCounter >= Plugin.Instance.Config.VoteDuration + 1)
                {
                    if (VoteAPI.CurrentVote.Callback == null)
                    {
                        var timerBroadcast = Plugin.Instance.Translation.Results;
                        foreach (var kv in VoteAPI.CurrentVote.Options)
                        {
                            timerBroadcast += Plugin.Instance.Translation.OptionAndCounter.Replace("%Option%", kv.Value)
                                .Replace("%OptionKey%", kv.Key).Replace("%Counter%",
                                    VoteAPI.CurrentVote.Counter[kv.Key].ToString());
                            textsize = timerBroadcast.Length / 10;
                        }

                        Map.Broadcast(5, "<size=" + (48 - textsize) + ">" + timerBroadcast + "</size>");
                    }
                    else
                    {
                        VoteAPI.CurrentVote.Callback.Invoke(VoteAPI.CurrentVote);
                    }

                    StopVote();
                    yield break;
                }

                {
                    var timerBroadcast = firstBroadcast + "\n";
                    foreach (var kv in VoteAPI.CurrentVote.Options)
                    {
                        timerBroadcast += Plugin.Instance.Translation.OptionAndCounter.Replace("%Option%", kv.Value)
                            .Replace("%OptionKey%", kv.Key).Replace("%Counter%",
                                VoteAPI.CurrentVote.Counter[kv.Key].ToString());
                        textsize = timerBroadcast.Length / 10;
                    }

                    Map.Broadcast(1, "<size=" + (48 - textsize) + ">" + timerBroadcast + "</size>");
                }
                timerCounter++;
                yield return Timing.WaitForSeconds(1f);
            }
        }

        public static string Rigging(string argument)
        {
            if (VoteAPI.CurrentVote == null) return "vote not active";
            if (!VoteAPI.CurrentVote.Options.ContainsKey(argument))
                return Plugin.Instance.Translation.NoOptionAvailable;
            VoteAPI.CurrentVote.Counter[argument]++;
            return "vote added";
        }
    }
}

public delegate void CallvoteFunction(Vote vote);