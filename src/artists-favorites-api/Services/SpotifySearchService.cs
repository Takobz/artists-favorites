using artists_favorites_api.Clients.Spotify;
using artists_favorites_api.Extensions;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries;

namespace artists_favorites_api.Services 
{
    public interface ISpotifySearchService 
    {
        Task<IEnumerable<SearchArtistResult>> GetArtistsByName(string artistName);
    }

    public class SpotifySearchService(ISpotifySearchClient spotifyClient) : ISpotifySearchService 
    {
        private readonly ISpotifySearchClient _spotifyClient = spotifyClient;

        public async Task<IEnumerable<SearchArtistResult>> GetArtistsByName(string artistName) 
        {
            var artistsSearchResult = await _spotifyClient.GetArtist(artistName);

            return artistsSearchResult == null || artistsSearchResult.SearchFoundNoArtists() ?
                [] : artistsSearchResult.ResultsFromSearchResponse();
        }
    }
}