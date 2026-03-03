using Callvote.API.Delegates;
using Callvote.API.Features.Commands;

namespace Callvote.API.Features.Votes
{
    /// <summary>
    /// Represents the type that manages and creates a <see cref="VoteOption"/> Option.
    /// Responsible for the <see cref="VoteOption"/> Option, Detail and Command Registration.
    /// </summary>
    /// <remarks>The Command syntax might change if a command is already registered.</remarks>
    public class VoteOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoteOption"/> class with the specified option and detail.
        /// </summary>
        /// <param name="option">The <see cref="VoteOption"/> Option.</param>
        /// <param name="detail">The <see cref="VoteOption"/> <see cref="Detail"/>.</param>
        /// <param name="customResponseHandler">The .</param>
        public VoteOption(string option, string detail, ResponseHandler customResponseHandler = null)
        {
            this.Option = option;
            this.Detail = detail;
            this.VoteCommand = new(this, customResponseHandler);
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
        /// Gets the <see cref="Commands.VoteCommand"/> associated with this <see cref="VoteOption"/>.
        /// </summary>
        public VoteCommand VoteCommand { get; }
    }
}
