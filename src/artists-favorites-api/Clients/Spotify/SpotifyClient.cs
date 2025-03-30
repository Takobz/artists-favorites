namespace artists_favorites_api.Clients.Spotify 
{
    interface ISpotifyClient 
    {
        Task<string> GetArtist(string artistName);
    }

    public class SpotifyClient(IHttpClientFactory clientFactory) : ISpotifyClient 
    {
        private readonly HttpClient _httpClient = clientFactory.CreateClient();

        public async Task<string> GetArtist(string artistName)
        {
            return await Task.FromResult(
                _httpClient.DefaultRequestHeaders.TryGetValues("Authorization", out var x) ?
                x.First() : "Whoops");
        }
    }
}