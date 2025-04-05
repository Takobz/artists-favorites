using artists_favorites_api.Helpers;

namespace artists_favorites_api.Clients.Spotify 
{
    public interface ISpotifySearchClient 
    {
        Task<string> GetArtist(string artistName);
    }

    public class SpotifySearchClient(HttpClient httpClient) : ISpotifySearchClient 
    {
        public async Task<string> GetArtist(string artistName)
        {
            var searchQuery = SpotifyV1QueryBuilder.SearchArtist(artistName);
            var result = await httpClient.GetAsync(searchQuery);
            if (result.IsSuccessStatusCode)
            {
                //var 
            }

            return await Task.FromResult(
                httpClient.DefaultRequestHeaders.TryGetValues("Authorization", out var x) ?
                x.First() : await result.Content.ReadAsStringAsync());
        }
    }
}