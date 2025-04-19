using artists_favorites_api.Models.DTOs.Requests;
using artists_favorites_api.AuthProviders;
using artists_favorites_api.Extensions;
using artists_favorites_api.Exceptions;
using artists_favorites_api.Constants;
using artists_favorites_api.Services;
using System.Net;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries;

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
                CreatePlaylistRequest request,
                HttpRequest httpRequest,
                ISpotifyPlaylistService spotifyPlaylistService) => 
            {
                if (httpRequest.Headers.TryGetValue("Authorization", out var bearerToken) &&
                    !string.IsNullOrEmpty(bearerToken.FirstOrDefault())) 
                {
                    var accessTokenWithBearerPrefix = bearerToken.First();
                    var accessToken = accessTokenWithBearerPrefix!.Substring(7); //for Bearer soMeToken gets soMeToken substring
                    var createEmptyPlaylistCommand = request.ToCreateEmptyPlaylistCommand(accessToken);
                    var response = await spotifyPlaylistService.CreateEmptyPlaylist(createEmptyPlaylistCommand);

                    return Results.Created(string.Empty, response);
                }
                else 
                {
                    throw new ArtistsFavoritesHttpException(
                        (int)HttpStatusCode.Unauthorized,
                        FriendlyErrorMessage.UnauthorisedAccess("User Playlists")
                    );
                }
            })
            .WithName("CreatePlaylist")
            .WithOpenApi();

            routeBuilder.MapGet("v1/playlist/liked/{artistName}", async (
                string artistName,
                HttpRequest httpRequest,
                ISpotifyTrackService spotifyTrackService
            ) => 
            {
                if (httpRequest.Headers.TryGetValue("Authorization", out var bearerToken) &&
                    !string.IsNullOrEmpty(bearerToken.FirstOrDefault())) 
                {
                    var accessTokenWithBearerPrefix = bearerToken.First();
                    var accessToken = accessTokenWithBearerPrefix!.Substring(7); //for Bearer soMeToken gets soMeToken substring
                    var response = await spotifyTrackService.GetUserSavedTracks(new GetSavedTracksQuery(
                        accessToken,
                        artistName
                    ));

                    return Results.Ok(response);
                }
                else 
                {
                    throw new ArtistsFavoritesHttpException(
                        (int)HttpStatusCode.Unauthorized,
                        FriendlyErrorMessage.UnauthorisedAccess("User Liked Songs")
                    );
                }
            })
            .WithName("GetLikeSongsByArtist")
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
                GetUserTokenRequest body,
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