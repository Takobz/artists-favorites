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

        //TODO: move this to SpotifyResourceClient make this a SpotifyAuthClient
        public async Task<string> GetArtist(string artistName)
        {
            var result = await httpClient.GetAsync("/");

            return await Task.FromResult(
                httpClient.DefaultRequestHeaders.TryGetValues("Authorization", out var x) ?
                x.First() : await result.Content.ReadAsStringAsync());
        }
    }
}