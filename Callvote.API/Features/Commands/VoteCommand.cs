using Callvote.API.Delegates;
using Callvote.API.Features.Votes;

namespace Callvote.API.Features.Commands
{
    /// <summary>
    /// Represents the class for creating a generic command for the Vote.
    /// </summary>
    public class VoteCommand
    {
        private readonly VoteOption voteOption;
        private readonly ResponseHandler responseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoteCommand"/> class.
        /// </summary>
        /// <param name="voteOption">The command.</param>
        /// <param name="responseHandler">The response handler for the command.</param>
        internal VoteCommand(VoteOption voteOption, ResponseHandler responseHandler)
        {
            this.voteOption = voteOption;
            this.responseHandler = responseHandler;
            this.Command = voteOption.Option;
            this.Description = voteOption.Detail;
        }

        /// <summary>
        /// Gets or sets the command name.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets the description of the vote command.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Executes a command using the specified arguments and provides a response message indicating the result.
        /// </summary>
        /// <param name="user">The user that supplies context and state information for the command execution. Can be null.</param>
        /// <param name="response">When this method returns, contains the response message generated during command execution.</param>
        /// <returns>true if the command was executed successfully; otherwise, false.</returns>
        public virtual bool OnCommandExecuted(UserIndentifier user, out string response)
        {
            if (VoteHandler.CurrentVote == null)
            {
                response = "There is no active vote!";
                return false;
            }

            var handler = this.responseHandler ?? VoteHandler.CurrentVote.VoteCommandResponse;

            (bool, string)? results = handler?.Invoke(user, this.voteOption);

            if (!results.HasValue)
            {
                response = "The response method wasn't set!";
                return false;
            }

            (bool sucess, response) = results.Value;
            return sucess;
        }
    }
}
