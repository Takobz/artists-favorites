using artists_favorites_api.Services;

namespace artists_favorites_api.Routes 
{
    public static class SpotifyOpenAPIRoutes 
    {
        public static IEndpointRouteBuilder MapSpotifyV1Routes(this IEndpointRouteBuilder routeBuilder) 
        {
            routeBuilder.MapGet("v1/search/{artistName}", async (string artistName, ISpotifySearchService spotifySearchService) => {
                var artistSearchResults = await spotifySearchService.GetArtistsByName(artistName);
                
                return artistSearchResults.Any() ? 
                    Results.Ok(artistSearchResults) : Results.NotFound();
            })
            .WithName("SearchArtist")
            .WithOpenApi();

            return routeBuilder;
        }
    }
}