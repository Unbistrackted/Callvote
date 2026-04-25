using Callvote.API.Features.Votes;

namespace Callvote.Features
{
    /// <summary>
    /// Represents the enumeration for <see cref="Vote"/> types.
    /// </summary>
    public enum VoteType
    {
        /// <summary>A<see cref="Vote"/> that has 2 options.</summary>
        Binary,

        /// <summary>A<see cref="Vote"/> that has custom ammount of votes.</summary>
        Custom,
    }
}
