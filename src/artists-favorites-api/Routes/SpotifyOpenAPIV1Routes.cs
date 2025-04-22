using artists_favorites_api.Models.DTOs.Requests;
using artists_favorites_api.Extensions;
using artists_favorites_api.Constants;
using artists_favorites_api.Services;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries;
using artists_favorites_api.Authentication;

namespace artists_favorites_api.Routes 
{
    public static class SpotifyOpenAPIV1Routes 
    {
        //TODO: Describe known Responses and Status with OpenAPI Swagger.
        public static IEndpointRouteBuilder MapSpotifyV1Routes(this IEndpointRouteBuilder routeBuilder) 
        {
            routeBuilder.MapGet("v1/search/{artistName}", async (
                string artistName, 
                ISpotifySearchService spotifySearchService) => 
            {
                var artistSearchResults = await spotifySearchService.GetArtistsByName(artistName);
                return artistSearchResults.Any() ? 
                    Results.Ok(artistSearchResults.Select(asr => asr.ToSearchArtistResponseDTO())) : Results.NotFound();
            })
            .WithName("SearchArtist")
            .WithOpenApi();

            routeBuilder.MapPost("v1/playlist/create", async (
                CreatePlaylistRequestDTO request,
                ISpotifyPlaylistService spotifyPlaylistService) => 
            {
                var createEmptyPlaylistCommand = request.ToCreateEmptyPlaylistCommand();
                var response = await spotifyPlaylistService.CreateEmptyPlaylist(createEmptyPlaylistCommand);
                return Results.Created(string.Empty, response);
            })
            .RequireAuthorization(SpotifyAuthenticationCustomPolicies.SpotifyUser)
            .WithName("CreatePlaylist")
            .WithOpenApi();

            routeBuilder.MapPut("v1/playlist/{playlistId}", async (
                string playlistId,
                AddItemsToPlaylistRequestDTO request,
                ISpotifyPlaylistService spotifyPlaylistService
             ) => 
            {
                var addItemsCommand = request.ToAddItemsCommand(playlistId);
                var response = await spotifyPlaylistService.AddItemsToPlaylist(addItemsCommand);
                return Results.Created(string.Empty, response);
            })
            .RequireAuthorization(SpotifyAuthenticationCustomPolicies.SpotifyUser)
            .WithName("AddTracksToPlaylist")
            .WithOpenApi();

            routeBuilder.MapGet("v1/playlist/liked/{artistEntityId}", async (
                string artistEntityId,
                ISpotifyTrackService spotifyTrackService
            ) => 
            {
                var response = await spotifyTrackService.GetUserSavedTracks(new GetSavedTracksQuery(
                    artistEntityId
                ));

                return Results.Ok(response);
            })
            .RequireAuthorization(SpotifyAuthenticationCustomPolicies.SpotifyUser)
            .WithName("GetLikedSongsByArtist")
            .WithOpenApi();

            return routeBuilder;
        }
    }
}