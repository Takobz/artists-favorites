using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using artists_favorites_api.Clients.Spotify;
using artists_favorites_api.Constants;
using artists_favorites_api.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace artists_favorites_api.Authentication 
{
    public class SpotifyAuthenticationHandler(
        IOptionsMonitor<SpotifyAuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder urlEncoder,
        ISpotifyUserClient spotifyUserClient,
        IHttpContextAccessor httpContextAccessor) 
        : AuthenticationHandler<SpotifyAuthenticationSchemeOptions>(options, loggerFactory, urlEncoder)
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var httpContext = httpContextAccessor.HttpContext ?? throw new ArtistsFavoritesHttpException(
                (int)HttpStatusCode.InternalServerError,
                FriendlyErrorMessage.GenericInternalAppError
            );

            var spotifyAccessToken = ExtractBearerTokenFromHeader(httpContext.Request.Headers);
            if (string.IsNullOrEmpty(spotifyAccessToken)) 
            {
                return AuthenticateResult.Fail(new ArtistsFavoritesHttpException(
                    (int)HttpStatusCode.Unauthorized,
                    FriendlyErrorMessage.MissingAuthorizationBearerHeader
                ));
            }

            var userProfile = await spotifyUserClient.GetCurrentUserProfileResponse(spotifyAccessToken);
            if (userProfile == null)
            {
                return AuthenticateResult.Fail(new ArtistsFavoritesHttpException(
                    (int)HttpStatusCode.Unauthorized,
                    FriendlyErrorMessage.MissingAuthorizationBearerHeader
                ));
            }

            var claimsIdentity = new ClaimsIdentity([
                new Claim(SpotifyAuthenticationCustomClaims.SpotifyAccessToken, spotifyAccessToken),
                new Claim(ClaimTypes.Name, userProfile.DisplayName),
                new Claim(ClaimTypes.Email, userProfile.Email)
            ]);

            return AuthenticateResult.Success(new AuthenticationTicket(
                new ClaimsPrincipal(new ClaimsPrincipal(claimsIdentity)),
                new AuthenticationProperties(),
                SpotifyAuthenticationDefaults.AuthenticationScheme
            ));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            //TODO: Handle 401
              await Task.FromException(new ArtistsFavoritesHttpException(
                (int)HttpStatusCode.Unauthorized,
                FriendlyErrorMessage.UnauthorisedAccess()
            ));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            //TODO Handle 403
            await Task.FromException(new ArtistsFavoritesHttpException(
                (int)HttpStatusCode.Forbidden,
                FriendlyErrorMessage.ForbiddenAccess
            ));
        }

        private static string ExtractBearerTokenFromHeader(IHeaderDictionary requestHeader) 
        {
            if (requestHeader.TryGetValue("Authorization", out var accessTokenWithBearerPrefixStringValue) &&
                !string.IsNullOrEmpty(accessTokenWithBearerPrefixStringValue)
            )
            {
                //remove prefix "Bearer "
                var accessTokenWithBearerPrefix = accessTokenWithBearerPrefixStringValue.First();
                return accessTokenWithBearerPrefix![7..];
            }

            return string.Empty;
        }
    }

    public class SpotifyAuthenticationSchemeOptions : AuthenticationSchemeOptions 
    {
        
    }

    public static class SpotifyAuthenticationDefaults 
    {
        public const string AuthenticationScheme = "SpotifyUserDelegationAuthentication";
    }
}