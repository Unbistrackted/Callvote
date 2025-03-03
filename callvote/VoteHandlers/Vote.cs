using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace callvote.VoteHandlers
{
    public class Vote
    {
        public string Question; 
        public Dictionary<string, string> Options; //Option and Option Description
        public Dictionary<string, string> Votes; // Player and vote type
        public Dictionary<string, int> Counter; //Option and Votes<int> to that Option
        public Timer Timer;
        public CallvoteFunction Callback;

        public Vote(string question, Dictionary<string, string> options)
        {
            this.Question = question;
            this.Options = options;
            this.Votes = new Dictionary<string, string>();
            this.Counter = new Dictionary<string, int>();
            foreach (string option in options.Keys)
            {
                Counter[option] = 0;
            }
        }

        // Allow Votes and Counter to be passed in and saved by reference for Event code
        public Vote(string question, Dictionary<string, string> options, Dictionary<string, string> votes, Dictionary<string, int> counter)
        {
            this.Question = question;
            this.Options = options;
            this.Votes = votes;
            this.Counter = counter;
            foreach (string option in options.Keys)
            {
                Counter[option] = 0;
            }
        }
    }
}
