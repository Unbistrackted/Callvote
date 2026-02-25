using System;
using Callvote.API.Votes.Delegates;
using CommandSystem;
using LabApi.Features.Wrappers;
using RemoteAdmin;

namespace Callvote.API.Votes
{
    /// <summary>
    /// Represents the type that manages and creates a <see cref="VoteOption"/> Option.
    /// Responsible for the <see cref="VoteOption"/> Option, Detail and Command Registration.
    /// </summary>
    /// <remarks>The Command syntax might change if a command is already registered.</remarks>
    public class VoteOption : VoteCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoteOption"/> class with the specified option and detail.
        /// </summary>
        /// <param name="option">The <see cref="VoteOption"/> Option.</param>
        /// <param name="detail">The <see cref="VoteOption"/> <see cref="Detail"/>.</param>
        /// <param name="responseHandler">The response handler for the command.</param>
        public VoteOption(string option, string detail, ResponseHandler responseHandler = null)
            : base(option, responseHandler)
        {
            this.Option = option;
            this.Detail = detail;
        }

        /// <summary>
        /// Gets the <see cref="VoteOption"/> Option.
        /// </summary>
        public string Option { get; init; }

        /// <summary>
        /// Gets the <see cref="VoteOption"/> Detail, which is the description of the option.
        /// </summary>
        public string Detail { get; init; }

        /// <summary>
        /// Gets a value indicating whether the command was registered.
        /// </summary>
        public bool IsCommandRegistered => QueryProcessor.DotCommandHandler.TryGetCommand(this.Command, out _);

        /// <inheritdoc/>
        public override bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            (bool, string)? responseHandler = this.ResponseHandler?.Invoke(Player.Get(sender)?.ReferenceHub, this);

            if (!responseHandler.HasValue)
            {
                response = "The response method wasn't set!";
                return false;
            }

            (bool sucess, response) = responseHandler.Value;
            return sucess;
        }

        /// <summary>
        /// Registers the <see cref="VoteCommand"/>.
        /// </summary>
        /// <param name="vote">The <see cref="Vote"/> that the <see cref="VoteOption"/> is being registed .</param>
        internal void Register(Vote vote)
        {
            if (this.ResponseHandler == null)
            {
                this.ResponseHandler = vote.VoteCommandResponse;
            }

            vote.RegisterVoteOption(this);
        }

        /// <summary>
        /// Unregisters the <see cref="VoteCommand"/>.
        /// </summary>
        internal void UnregisterCommand()
        {
            if (this.IsCommandRegistered)
            {
                QueryProcessor.DotCommandHandler.UnregisterCommand(this);
            }
        }
    }
}
