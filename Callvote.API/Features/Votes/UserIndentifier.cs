using System;

namespace Callvote.API.Features.Votes
{
    /// <summary>
    /// Represents the type that manages and creates the <see cref="UserIndentifier"/>.
    /// </summary>
    public class UserIndentifier : IEquatable<UserIndentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserIndentifier"/> class.
        /// </summary>
        /// <param name="userId"><see cref="UserId"/>.</param>
        /// <param name="name"><see cref="Username"/>.</param>
        /// <param name="uniqueId"><see cref="UniqueUserId"/>.</param>
        public UserIndentifier(int userId, string name, string uniqueId)
        {
            this.UserId = userId;
            this.Username = name;
            this.UniqueUserId = uniqueId;
        }

        /// <summary>
        /// Gets the player's id.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets the player's name.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets the player's unique id.
        /// </summary>
        /// <remarks>This can be a steamid, or any type of unique identifier for that specific player.</remarks>
        public string UniqueUserId { get; }

#if !BAREBONES
        /// <summary>
        /// Implicitly converts a ReferenceHub instance to a UserIndentifier instance, enabling seamless use of user
        /// identification data where a UserIndentifier is required.
        /// </summary>
        /// <param name="referenceHub">The ReferenceHub instance containing the player's identification information to be converted.</param>
        public static implicit operator UserIndentifier(ReferenceHub referenceHub) => new(referenceHub.PlayerId, referenceHub.nicknameSync.MyNick, referenceHub.authManager.UserId);
#endif

        /// <summary>
        /// Determines whether two instances of the <see cref="UserIndentifier"/> class are equal.
        /// </summary>
        /// <param name="left">The first <see cref="UserIndentifier"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UserIndentifier"/> instance to compare.</param>
        /// <returns>true if the two <see cref="UserIndentifier"/> instances are equal; otherwise, false.</returns>
        public static bool operator ==(UserIndentifier left, UserIndentifier right) => left.Equals(right);

        /// <summary>
        /// Determines whether two instances of the <see cref="UserIndentifier"/> class are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="UserIndentifier"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UserIndentifier"/> instance to compare.</param>
        /// <returns>true if the two <see cref="UserIndentifier"/> instances not are equal; otherwise, false.</returns>
        public static bool operator !=(UserIndentifier left, UserIndentifier right) => !left.Equals(right);

        /// <inheritdoc/>
        public bool Equals(UserIndentifier other) => this.UniqueUserId == other.UniqueUserId && this.UserId == other.UserId;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is UserIndentifier other && this.Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => this.UniqueUserId.GetHashCode();
    }
}
