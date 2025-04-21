using System.Net;
using System.Security.Claims;
using artists_favorites_api.Constants;
using artists_favorites_api.Exceptions;

namespace artists_favorites_api.Extensions 
{
    public static class ClaimsPrincipalExtensions 
    {
        public static string GetClaimValueByName(
            this ClaimsPrincipal user,
            string claimName)
        {
            var spotifyIdentity = user.Identities.FirstOrDefault(identity => identity.Claims.Any(
                claim => claim.Type == claimName
            )) ?? 
            throw new ArtistsFavoritesHttpException(
                    (int)HttpStatusCode.Unauthorized,
                    FriendlyErrorMessage.UnauthorisedAccess()
            );

            return spotifyIdentity.Claims
                .First(claim => claim.Type == claimName)
                .Value;
        }

        public static string GetSpotifyUserEntityId(this ClaimsPrincipal user){
            var spotifyIdentity = user.Identities.FirstOrDefault(identity => identity.Claims.Any(
                claim => claim.Type == SpotifyAuthenticationCustomClaims.SpotifyUserEntityId
            )) ?? 
            throw new ArtistsFavoritesHttpException(
                    (int)HttpStatusCode.Unauthorized,
                    FriendlyErrorMessage.UnauthorisedAccess()
            );

            return spotifyIdentity.Claims
                .First(claim => claim.Type == SpotifyAuthenticationCustomClaims.SpotifyUserEntityId)
                .Value;
        }
    }
}