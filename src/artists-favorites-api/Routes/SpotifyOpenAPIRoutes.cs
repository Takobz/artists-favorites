using artists_favorites_api.Services;
using artists_favorites_api.Extensions;
using artists_favorites_api.AuthProviders;
using artists_favorites_api.Constants;
using artists_favorites_api.Models.DTOs.Requests;

namespace artists_favorites_api.Routes 
{
    public static class SpotifyOpenAPIRoutes 
    {
        public static IEndpointRouteBuilder MapSpotifyV1Routes(this IEndpointRouteBuilder routeBuilder) 
        {
            routeBuilder.MapGet("v1/search/{artistName}", async (string artistName, ISpotifySearchService spotifySearchService) => {
                var artistSearchResults = await spotifySearchService.GetArtistsByName(artistName);
                return artistSearchResults.Any() ? 
                    Results.Ok(artistSearchResults.Select(asr => asr.ToSearchArtistResponseDTO())) : Results.NotFound();
            })
            .WithName("SearchArtist")
            .WithOpenApi();

            return routeBuilder;
        }

        public static IEndpointRouteBuilder MapSpotifyAuthRoutes(this IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapGet("initiate-authorize", async (ISpotifyAuthProvider authProvider) => {
                var response = await authProvider.InitiateAuthorizationRequest(SpotifyUserAuthorizationScopes.UserLibraryRead);
                return Results.Ok(response);
            })
            .WithName("InitiateAuthorize")
            .WithOpenApi();

            routeBuilder.MapPost("/spotify-user-token", async (
                GetUserTokenRequest body,
                ISpotifyAuthProvider authProvider) => {
                var accessTokenResponse = await authProvider.GetAuthorizationCodeAccessToken(body.AuthorizationCode); 
                return Results.Ok(accessTokenResponse.ToGetUserTokenResponseDTO());
            })
            .WithName("SpotifyAccessToken")
            .WithOpenApi();

            return routeBuilder;
        }
    }
}