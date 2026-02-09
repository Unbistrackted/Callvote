using Callvote.Commands.MiscellaneousCommands;
using RemoteAdmin;

namespace Callvote.Features
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
        public VoteOption(string option, string detail)
        {
            this.Option = option;
            this.Detail = detail;
            this.Command = new(option);
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
        /// Gets the <see cref="VoteOption"/> Command.
        /// </summary>
        public VoteCommand Command { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Command"/> was registered.
        /// </summary>
        public bool IsCommandRegistered => QueryProcessor.DotCommandHandler.TryGetCommand(this.Command.Command, out _);

        /// <summary>
        /// Registers the <see cref="Command"/>.
        /// </summary>
        internal void RegisterCommand()
        {
            if (this.IsCommandRegistered)
            {
                this.Command.Command = "cv" + this.Option;
            }

            QueryProcessor.DotCommandHandler.RegisterCommand(this.Command);
        }

        /// <summary>
        /// Unregisters the <see cref="Command"/>.
        /// </summary>
        internal void UnregisterCommand()
        {
            if (this.IsCommandRegistered)
            {
                QueryProcessor.DotCommandHandler.UnregisterCommand(this.Command);
            }
        }
    }
}
