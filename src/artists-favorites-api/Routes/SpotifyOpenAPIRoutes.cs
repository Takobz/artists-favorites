using artists_favorites_api.Models.DTOs.Requests;
using artists_favorites_api.Extensions;
using artists_favorites_api.Exceptions;
using artists_favorites_api.Constants;
using artists_favorites_api.Services;
using System.Net;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries;
using artists_favorites_api.Authentication;

namespace artists_favorites_api.Routes 
{
    public static class SpotifyOpenAPIRoutes 
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

            //TODO: Add authentication handler that extracts the bearer token
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
                return Results.Ok(accessTokenResponse.ToGetUserTokenResponseDTO());
            })
            .WithName("SpotifyAccessToken")
            .WithOpenApi();

            return routeBuilder;
        }
    }
}