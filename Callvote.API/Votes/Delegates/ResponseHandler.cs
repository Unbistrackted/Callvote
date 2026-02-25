namespace Callvote.API.Votes.Delegates
{
    /// <summary>
    /// Defines a method for returning a reponse in <see cref="VoteCommand"/>.
    /// </summary>
    /// <param name="player">The player who sent the command.</param>
    /// <param name="voteOption">The option that the player voted.</param>
    /// <returns>A tuple representing if it was sucessful and the response.</returns>
    public delegate (bool Sucess, string Response)? ResponseHandler(ReferenceHub player, VoteOption voteOption);
}
