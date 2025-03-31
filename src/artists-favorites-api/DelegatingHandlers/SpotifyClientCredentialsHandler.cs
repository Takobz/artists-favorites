using Microsoft.Extensions.Caching.Memory;
using artists_favorites_api.Constants;

namespace artists_favorites_api.DelegatingHandlers 
{
    public class SpotifyClientCredentialsHandler(IMemoryCache memoryCache) : DelegatingHandler
    {
        private const string _spotifyAccessToken = ApplicationConstants.SpotifyBasicAccessTokenCacheKey;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!memoryCache.TryGetValue(_spotifyAccessToken, out var accessToken)) 
            {
                
            }
            return await base.SendAsync(request, cancellationToken);
        } 
    }
}