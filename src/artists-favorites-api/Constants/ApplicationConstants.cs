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
    }

    public static class SpotifyUserAuthorizationCodes 
    {
        public const string Code = "code";
    }
}