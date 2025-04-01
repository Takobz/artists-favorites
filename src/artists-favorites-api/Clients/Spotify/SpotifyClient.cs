namespace artists_favorites_api.Clients.Spotify 
{
    public interface ISpotifyClient 
    {
        Task<string> GetArtist(string artistName);
    }

    public class SpotifyClient(HttpClient httpClient) : ISpotifyClient 
    {
        public const string SpotifyClientClient = "spotify-auth-client";

        //private readonly HttpClient _httpClient = clientFactory.CreateClient(SpotifyClientClient);

        public async Task<string> GetArtist(string artistName)
        {
            return await Task.FromResult(
                httpClient.DefaultRequestHeaders.TryGetValues("Authorization", out var x) ?
                x.First() : "Whoops");
        }
    }
}