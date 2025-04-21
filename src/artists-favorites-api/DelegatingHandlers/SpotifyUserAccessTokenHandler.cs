

using artists_favorites_api.Constants;
using artists_favorites_api.Exceptions;
using artists_favorites_api.Extensions;
namespace artists_favorites_api.DelegatingHandlers 
{
    public class SpotifyUserAccessTokenHandler(IHttpContextAccessor contextAccessor) : DelegatingHandler 
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (contextAccessor == null || contextAccessor?.HttpContext == null)
            {
                throw new ArtistsFavoritesHttpException();
            }
            
            var spotifyUserAccessToken = contextAccessor.HttpContext.User.GetClaimValueByName(
                SpotifyAuthenticationCustomClaims.SpotifyAccessToken
            );

            request.Headers.Remove("Authorization");
            request.Headers.Add("Authorization", $"Bearer {spotifyUserAccessToken}");
            
            return base.SendAsync(request, cancellationToken);
        }
    }
}
