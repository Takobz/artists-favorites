using artists_favorites_api.Models.ClientModels.Spotify;
using artists_favorites_api.Models.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using artists_favorites_api.Constants;
using System.Text;
using System.Text.Json;
using artists_favorites_api.Helpers;

namespace artists_favorites_api.AuthProviders 
{
    public interface ISpotifyAuthProvider 
    {
        /// <summary>
        /// Get Application level scoped access token
        /// </summary>
        /// <returns>Access Token that can be used by application, identifies the application</returns>
        Task<BaiscAccessTokenResponse> GetBaiscAccessToken();

        /// <summary>
        /// Initiates OAuth2.0 Authorization Code flow with Spotify Auth Server
        /// </summary>
        /// <param name="scope">Scope that the application is requesting on behalf of the user</param>
        /// <returns>Redirect URI that will be passed to the UI.</returns>
        Task<string> InitiateAuthorizationRequest(string scope);
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
                var base64ClientCredentials = GenerateBase64ClientCredentials();
                var accessTokenRequestParams = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } });

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base64ClientCredentials}");

                var response = await _httpClient.SendAsync(new HttpRequestMessage
                {
                    Content = accessTokenRequestParams,
                    RequestUri = new Uri($"{_spotifyOptions.SpotifyAuthUrl}/api/token"),
                    Method = HttpMethod.Post
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

        public async Task<string> InitiateAuthorizationRequest(string scope)
        {
            var request = new AuthorizeUserRequest(
                ClientId: _spotifyOptions.ClientId,
                RedirectUri: _spotifyOptions.RedirectUri,
                State: Guid.NewGuid().ToString(),
                Scope: scope);

            var response = await _httpClient.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri($"{_spotifyOptions.SpotifyAuthUrl}/authorize?{request.ToAuthorizeQueryParams()}"),
                Method = HttpMethod.Get
            });

            if (response.IsSuccessStatusCode)
            {
                return response?.Headers?.Location?.AbsoluteUri ?? await response?.Content?.ReadAsStringAsync();
            }

            throw new Exception("Failed To Initiate authorize request");
        }

        private string GenerateBase64ClientCredentials()
        {
            var byteClientCredentials = Encoding.UTF8.GetBytes($"{_spotifyOptions.ClientId}:{_spotifyOptions.ClientSecret}");
            return Convert.ToBase64String(byteClientCredentials);
        }
    }
}
