using System.Collections.Generic;
using System.Timers;

namespace Callvote.VoteHandlers
{
    public class Vote
    {
        public CallvoteFunction Callback;
        public Dictionary<string, int> Counter; //Option and Votes<int> to that Option
        public Dictionary<string, string> Options; //Option and Option Description
        public string Question;
        public Timer Timer;
        public Dictionary<string, string> Votes; // Player and vote type

        public Vote(string question, Dictionary<string, string> options)
        {
            Question = question;
            Options = options;
            Votes = new Dictionary<string, string>();
            Counter = new Dictionary<string, int>();
            foreach (var option in options.Keys) Counter[option] = 0;
        }

        // Allow Votes and Counter to be passed in and saved by reference for Event code
        public Vote(string question, Dictionary<string, string> options, Dictionary<string, string> votes,
            Dictionary<string, int> counter)
        {
            Question = question;
            Options = options;
            Votes = votes;
            Counter = counter;
            foreach (var option in options.Keys) Counter[option] = 0;
        }
    }
}