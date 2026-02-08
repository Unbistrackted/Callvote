using Callvote.Commands.VotingCommands;
using RemoteAdmin;

namespace Callvote.Features
{
    /// <summary>
    /// Represents the type that manages and creates a <see cref="Vote"/>.
    /// </summary>
    public class Vote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vote"/> class with the specified option and detail.
        /// </summary>
        /// <param name="option">The <see cref="Vote"/> Option.</param>
        /// <param name="detail">The <see cref="Vote"/> <see cref="Detail"/>.</param>
        public Vote(string option, string detail)
        {
            Option = option;
            Detail = detail;
            Command = new(option);
        }

        /// <summary>
        /// Gets the <see cref="Vote"/> Option.
        /// </summary>
        public string Option { get; init; }

        /// <summary>
        /// Gets the <see cref="Vote"/> Detail, which is the description of the option.
        /// </summary>
        public string Detail { get; init; }

        /// <summary>
        /// Gets the <see cref="Vote"/> Command.
        /// </summary>
        public VoteCommand Command { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Command"/> was registered.
        /// </summary>
        public bool IsCommandRegistered => QueryProcessor.DotCommandHandler.TryGetCommand(Command.Command, out _);

        /// <summary>
        /// Registers the <see cref="Command"/>.
        /// </summary>
        internal void RegisterCommand()
        {
            if (IsCommandRegistered)
            {
                Command.Command = "cv" + Option;
            }

            QueryProcessor.DotCommandHandler.RegisterCommand(Command);
        }

        /// <summary>
        /// Unregisters the <see cref="Command"/>.
        /// </summary>
        internal void UnregisterCommand()
        {
            if (IsCommandRegistered)
            {
                QueryProcessor.DotCommandHandler.UnregisterCommand(Command);
            }
        }
    }
}
