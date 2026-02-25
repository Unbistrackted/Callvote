using System;
using Callvote.API.Votes.Delegates;
using CommandSystem;

namespace Callvote.API.Votes
{
    /// <summary>
    /// Represents the class for creating a generic command for the Vote.
    /// </summary>
    public abstract class VoteCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoteCommand"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="responseHandler">The response handler.</param>
        internal VoteCommand(string command, ResponseHandler responseHandler)
        {
            this.Command = command;
            this.ResponseHandler = responseHandler;
        }

        /// <inheritdoc/>
        public string Command { get; set; }

        /// <inheritdoc/>
        public string[] Aliases { get; set; }

        /// <inheritdoc/>
        public string Description { get; } = "Work as a custom vote/option command";

        /// <summary>
        /// Gets or sets the delegate for the response handler.
        /// </summary>
        public ResponseHandler ResponseHandler { get; set; }

        /// <inheritdoc/>
        public abstract bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response);
    }
}
