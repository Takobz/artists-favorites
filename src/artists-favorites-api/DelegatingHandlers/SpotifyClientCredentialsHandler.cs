using Microsoft.Extensions.Caching.Memory;
using artists_favorites_api.Constants;
using artists_favorites_api.AuthProviders;
using artists_favorites_api.Models.ClientModels.Spotify;

namespace artists_favorites_api.DelegatingHandlers 
{
    public class SpotifyClientCredentialsHandler(
        IMemoryCache memoryCache,
        ISpotifyAuthProvider spotifyAuthProvider) : DelegatingHandler
    {
        private const string _spotifyAccessToken = ApplicationConstants.SpotifyBasicAccessTokenCacheKey;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!memoryCache.TryGetValue(_spotifyAccessToken, out BaiscAccessTokenResponse? accessToken)) 
            {
                accessToken = await spotifyAuthProvider.GetBaiscAccessToken();   
            }

            request.Headers.Add("Authorization", $"Bearer {accessToken!.AccessToken}");
            return await base.SendAsync(request, cancellationToken);
        } 
    }

    public class LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Begin Request with RequestURI: {RequestUri}", request.RequestUri);

            var result =  await base.SendAsync(request, cancellationToken);        

            logger.LogInformation("After Request RequestURI: {RequestUri}, StatusCode: {StatusCode}",
                request.RequestUri, result.StatusCode);

            return result;
        }
    }
}