using artists_favorites_api.Authentication;
using artists_favorites_api.Models.DTOs.Requests;
using artists_favorites_api.Extensions;
using artists_favorites_api.Constants;

namespace artists_favorites_api.Routes 
{
    public static class SpotifyAuthRoutes 
    {
        public static IEndpointRouteBuilder MapSpotifyAuthRoutes(this IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapGet("initiate-authorize", async (ISpotifyAuthProvider authProvider) => 
            {
                var scopes = SpotifyUserAuthorizationScopes.CreateFavoritesPlaylistSpotifyScopes();
                var response = await authProvider.InitiateAuthorizationRequest(scopes);
                return Results.Ok(response);
            })
            .WithName("InitiateAuthorize")
            .WithOpenApi();

            routeBuilder.MapPost("/spotify-user-token", async (
                GetUserTokenRequestDTO body,
                ISpotifyAuthProvider authProvider) => 
            {
                var accessTokenResponse = await authProvider.GetAuthorizationCodeAccessToken(body.AuthorizationCode); 
                return Results.Ok(accessTokenResponse!.ToGetUserTokenResponseDTO());
            })
            .WithName("SpotifyAccessToken")
            .WithOpenApi();

            return routeBuilder;
        }
    }
}