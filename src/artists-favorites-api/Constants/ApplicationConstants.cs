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

    public static class SpotifyAuthenticationCustomClaims 
    {
        /// <summary>
        /// Represents the name of a claim that will contain the user's access token. <br/>
        /// This access token will grant the app permission to do action on behalf of the user.
        /// </summary>
        public const string SpotifyAccessToken = "SpotifyAccessToken";
    }

    public static class SpotifyAuthenticationCustomPolicies 
    {
        public const string SpotifyUser = "SpotifyUser";
    }

    public static class FriendlyErrorMessage 
    {
        public const string GenericInternalAppError = "An Application Error Occured.";
        public const string ForbiddenAccess = $"User or Application presented a bad oauth token.";
        public const string BadOrInvalid = "Bad Or Expired Token";
        public const string MissingAuthorizationBearerHeader = "The HTTP request is missing the bearer token";

        /// <summary>
        /// Indicates insufficient or expired token to resources like spotify playlist, user profile etc.
        /// </summary>
        /// <param name="resourceName">The name of the resource, use very generic names that don't expose application internals</param>
        /// <returns>message to be displayed in an http response</returns>
        public static string UnauthorisedAccess(string resourceName = "") =>
            string.IsNullOrEmpty(resourceName) ? 
                $"Access Denied actioned is unauthorised":
                $"User or Application doesn't have enough permissions to access resource. {resourceName}";
    }
}