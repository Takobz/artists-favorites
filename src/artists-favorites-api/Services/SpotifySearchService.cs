using artists_favorites_api.Clients.Spotify;

namespace artists_favorites_api.Services 
{
    public interface ISpotifySearchService 
    {
        Task<IEnumerable<string>> GetArtistsByName(string artistName);
    }

    public class SpotifySearchService(ISpotifySearchClient spotifyClient) : ISpotifySearchService 
    {
        private readonly ISpotifySearchClient _spotifyClient = spotifyClient;

        public async Task<IEnumerable<string>> GetArtistsByName(string artistName) 
        {
            var token = await _spotifyClient.GetArtist(artistName);

            return [token];
        }
    }
}