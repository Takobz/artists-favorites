using artists_favorites_api.Models.ClientModels.Spotify;

namespace artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries 
{
    public class SearchArtistResult(
        string artistName,
        string artistSpotifyUrl,
        string artistImageUrl)
    {
        public string ArtistName { get; internal set; } = artistName;
        public string ArtistSpotifyUrl { get; internal set; } = artistSpotifyUrl;
        public string ArtistImageUrl { get; internal set; } = artistImageUrl;
    }
}