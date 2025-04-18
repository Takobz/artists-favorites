using artists_favorites_api.Models.ClientModels.Spotify;
using artists_favorites_api.Models.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using artists_favorites_api.Constants;
using System.Text;
using System.Text.Json;
using artists_favorites_api.Helpers;
using System.Net;
using artists_favorites_api.Models.DTOs.Responses;

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
        /// <param name="scopes">Scope that the application is requesting on behalf of the user</param>
        /// <returns>Redirect URI that will be passed to the UI.</returns>
        Task<InitiateAuthorizeResponse> InitiateAuthorizationRequest(string scopes);
        
        /// <summary>
        /// Get the Access token in exchange of the OAuth2.0 authorization code recieved. 
        /// </summary>
        /// <param name="authorizationCode">authorization code recieved after user authorization</param>
        /// <returns>Access token to access user resoucres</returns>
        Task<AuthorizationCodeAccessTokenResponse?> GetAuthorizationCodeAccessToken(string authorizationCode);
    }

    public class SpotifyAuthProvider(
        IOptions<SpotifyOptions> spotifyOptions,
        IMemoryCache memoryCache,
        HttpClient httpClient) : ISpotifyAuthProvider 
    {
        private readonly SpotifyOptions _spotifyOptions = spotifyOptions.Value;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly HttpClient _httpClient = httpClient;

        private const string _spotifyAccessToken = ApplicationConstants.SpotifyBasicAccessTokenCacheKey;

        //TODO: Move BaseUri sets to a delegating handler.
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

        public async Task<InitiateAuthorizeResponse> InitiateAuthorizationRequest(string scopes)
        {
            var request = new AuthorizeUserRequest(
                ClientId: _spotifyOptions.ClientId,
                RedirectUri: _spotifyOptions.RedirectUri,
                State: Guid.NewGuid().ToString(),
                Scope: scopes);

            var response = await _httpClient.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri($"{_spotifyOptions.SpotifyAuthUrl}/authorize?{request.ToAuthorizeQueryParams()}"),
                Method = HttpMethod.Get
            });

            if (response.StatusCode == HttpStatusCode.RedirectMethod ||
                response.StatusCode == HttpStatusCode.Redirect)
            {
                var spotifyAuthorizeUrl = response?.Headers?.Location?.AbsoluteUri ?? string.Empty;
                return new InitiateAuthorizeResponse(spotifyAuthorizeUrl);
            }

            throw new Exception("Failed To Initiate authorize request");
        }

        public async Task<AuthorizationCodeAccessTokenResponse?> GetAuthorizationCodeAccessToken(string authorizationCode)
        {
            var base64ClientCredentials = GenerateBase64ClientCredentials();
            var request = new FormUrlEncodedContent(new Dictionary<string, string> 
            {
                { "grant_type", "authorization_code" },
                { "code", authorizationCode },
                { "redirect_uri", _spotifyOptions.RedirectUri }
            });

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base64ClientCredentials}");
            var response = await _httpClient.SendAsync(new HttpRequestMessage 
            {
                RequestUri = new Uri($"{_spotifyOptions.SpotifyAuthUrl}/api/token"),
                Method = HttpMethod.Post,
                Content = request
            });

            if (response.IsSuccessStatusCode) 
            {
                return await JsonSerializer.DeserializeAsync<AuthorizationCodeAccessTokenResponse>(
                    await response.Content.ReadAsStreamAsync()
                );
            }

            throw new Exception($"Failed to get access token for authorization code: {authorizationCode}");
        }

        private string GenerateBase64ClientCredentials()
        {
            var byteClientCredentials = Encoding.UTF8.GetBytes($"{_spotifyOptions.ClientId}:{_spotifyOptions.ClientSecret}");
            return Convert.ToBase64String(byteClientCredentials);
        }
    }
}
