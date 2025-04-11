using artists_favorites_api.Services;
using artists_favorites_api.Extensions;
using artists_favorites_api.AuthProviders;
using artists_favorites_api.Constants;

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

            //This should be a Front end endpoint and present a code here for the access token
            routeBuilder.MapGet("/spotify-user-token", (HttpRequest request) => {
                //TODO: get code and get access token for user.
                var code = request.Path.Value;
                return Results.Ok(code);
            })
            .WithName("SpotifyCallback")
            .WithOpenApi();

            return routeBuilder;
        }
    }
}