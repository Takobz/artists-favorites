using artists_favorites_api.Models.ClientModels.Spotify;
using artists_favorites_api.Models.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using artists_favorites_api.Constants;
using System.Text;
using System.Text.Json;

namespace artists_favorites_api.AuthProviders 
{
    interface ISpotifyAuthProvider 
    {
        Task<BaiscAccessTokenResponse> GetBaiscAccessToken();
    }

    public class SpotifyAuthProvider(
        IOptions<SpotifyOptions> spotifyOptions,
        IMemoryCache memoryCache,
        IHttpClientFactory httpClientFactory) : ISpotifyAuthProvider 
    {
        private readonly SpotifyOptions _spotifyOptions = spotifyOptions.Value;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

        private const string _spotifyAccessToken = ApplicationConstants.SpotifyBasicAccessTokenCacheKey;

        public async Task<BaiscAccessTokenResponse> GetBaiscAccessToken()
        {
            if (!_memoryCache.TryGetValue(_spotifyAccessToken, out BaiscAccessTokenResponse? accessToken))
            {
                var byteClientCredentials = Encoding.UTF8.GetBytes($"{_spotifyOptions.ClientId}:{_spotifyOptions.ClientSecret}");
                var base64ClientCredentials = Convert.ToBase64String(byteClientCredentials);
                var accessTokenRequestParams = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } });

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base64ClientCredentials}");

                var response = await _httpClient.SendAsync(new HttpRequestMessage()
                {
                    Content = accessTokenRequestParams,
                    RequestUri = new Uri(_spotifyOptions.SpotifyAuthUrl)
                });

                if (response.IsSuccessStatusCode)
                {
                    accessToken = await JsonSerializer.DeserializeAsync<BaiscAccessTokenResponse>(await response.Content.ReadAsStreamAsync());
                    _memoryCache.Set(_spotifyAccessToken, accessToken);
                }
                else throw new Exception("Failed to get access token from spotify api.");
            }

            if (accessToken == null) throw new Exception("Can't continue without the access token");

            return accessToken;
        }
    }
}
