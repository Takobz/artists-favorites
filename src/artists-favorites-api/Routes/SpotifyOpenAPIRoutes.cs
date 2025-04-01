using artists_favorites_api.Services;

namespace artists_favorites_api.Routes 
{
    public static class SpotifyOpenAPIRoutes 
    {
        public static IEndpointRouteBuilder MapSpotifyRoutes(this WebApplication routeBuilder) {

            routeBuilder.MapGet("/search/{artistName}", async (string artistName, ISpotifySearchService spotifySearchService) => {
                var token = await spotifySearchService.GetArtistsByName(artistName);
                Results.Ok(token);
            })
            .WithName("SearchArtist")
            .WithOpenApi();

            return routeBuilder;
        }
    }
}