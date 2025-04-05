namespace artists_favorites_api.Models.Options 
{
    public class SpotifyOptions 
    {
        public const string Section = "SpotifyOptions";
        
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string SpotifyAuthUrl { get; set; } = string.Empty;
        public string SpotifySearchV1Url { get; set; } = string.Empty;
    }
}