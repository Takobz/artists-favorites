namespace artists_favorites_api.Constants 
{
    public static class ApplicationConstants 
    {
        public const string SpotifyBasicAccessTokenCacheKey = "spotify-web-api-token";
    }

    public static class SpotifyUserAuthorizationScopes 
    {
        public const string PlayListModifyPublic = "playlist-modify-public";
        public const string UserLibraryRead = "user-library-read";
        public const string UserReadEmail = "user-read-email";

        public static string CreateFavoritesPlaylistSpotifyScopes() =>
            $"{PlayListModifyPublic} {UserLibraryRead} {UserReadEmail}";
    }

    public static class SpotifyUserAuthorizationCodes 
    {
        public const string Code = "code";
    }

    public static class FriendlyErrorMessage 
    {
        public const string GenericInternalAppError = "An Application Error Occured.";
        public const string ForbiddenAccess = $"User or Application presented a bad oauth token.";

        /// <summary>
        /// Indicates insufficient or expired token to resources like spotify playlist, user profile etc.
        /// </summary>
        /// <param name="resourceName">The name of the resource, use very generic names that don't expose application internals</param>
        /// <returns>message to be displayed in an http response</returns>
        public static string UnauthorisedAccess(string resourceName) =>
            $"User or Application doesn't have enough permissions to access resource. {resourceName}";
    }
}