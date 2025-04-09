using System.Text.Json;
using artists_favorites_api.Helpers;
using artists_favorites_api.Models.ClientModels.Spotify;

namespace artists_favorites_api.Clients.Spotify 
{
    public interface ISpotifySearchClient 
    {
        Task<SpotifySearchResponses?> GetArtist(string artistName);
    }

    public class SpotifySearchClient(HttpClient httpClient) : ISpotifySearchClient 
    {
        public async Task<SpotifySearchResponses?> GetArtist(string artistName)
        {
            var searchQuery = SpotifyV1QueryBuilder.SearchArtist(artistName);
            var result = await httpClient.GetAsync(searchQuery);
            if (!result.IsSuccessStatusCode)
            {
                return default; 
            }

            return await JsonSerializer.DeserializeAsync<SpotifySearchResponses>(
                await result.Content.ReadAsStreamAsync());
        }
    }
}