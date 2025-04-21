using System.Net;
using artists_favorites_api.Constants;

namespace artists_favorites_api.Exceptions 
{
    /// <summary>
    /// Thrown when an application error happens and we need to respond back to the caller.
    /// This will be handled by a global exception handler that can respond in HTTP to the caller.
    /// </summary>
    public class ArtistsFavoritesHttpException(
        int httpStatusCode = (int)HttpStatusCode.InternalServerError,
        string message = FriendlyErrorMessage.GenericInternalAppError
        ) : Exception(message) 
    {
        /// <summary>
        /// Http status codes as defined by: https://datatracker.ietf.org/doc/html/rfc7231
        /// </summary>
        public int HttpStatusCode { get; internal set; } = httpStatusCode;
    }
}