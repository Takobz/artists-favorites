using Microsoft.Extensions.Caching.Memory;
using artists_favorites_api.Constants;
using artists_favorites_api.Models.ClientModels.Spotify;
using artists_favorites_api.Authentication;

namespace artists_favorites_api.DelegatingHandlers 
{
    public class SpotifyClientCredentialsDelegatingHandler(
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
}