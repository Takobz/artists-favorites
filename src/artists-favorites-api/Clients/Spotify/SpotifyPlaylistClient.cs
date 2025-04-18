using System.Net;
using System.Text.Json;
using artists_favorites_api.Constants;
using artists_favorites_api.Exceptions;
using artists_favorites_api.Models.ClientModels.Spotify;

namespace artists_favorites_api.Clients.Spotify 
{
    public interface ISpotifyPlaylistClient 
    {
        Task<SpotifyPlaylistResponse> CreatePlaylist(string userId, string accessToken, CreatePlaylist request);
    }

    public class SpotifyPlaylistClient(
        HttpClient httpClient,
        ILogger<SpotifyPlaylistClient> logger) : ISpotifyPlaylistClient
    {
        public async Task<SpotifyPlaylistResponse> CreatePlaylist(
            string userId,
            string accessToken,
            CreatePlaylist request)
        {
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var response = await httpClient.PostAsJsonAsync($"v1/users/{userId}/playlists", request);
            if (response.IsSuccessStatusCode)
            {
                var createdPlaylist =  await JsonSerializer.DeserializeAsync<SpotifyPlaylistResponse>(
                    await response.Content.ReadAsStreamAsync()
                );

                if (createdPlaylist == null)
                {
                    logger.LogError("Failed to deserialize Http Content: {HttpResponseContent} to class: {ClassType}",
                        await response.Content.ReadAsStringAsync() ?? string.Empty,
                        nameof(SpotifyPlaylistResponse)
                    );

                    throw new ArtistsFavoritesHttpException(
                        (int)HttpStatusCode.InternalServerError,
                        FriendlyErrorMessage.GenericInternalAppError
                    );
                }
                return createdPlaylist;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ArtistsFavoritesHttpException(
                    (int)HttpStatusCode.BadRequest,
                    FriendlyErrorMessage.BadOrInvalid
                );
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new ArtistsFavoritesHttpException(
                    (int)HttpStatusCode.Forbidden,
                    FriendlyErrorMessage.ForbiddenAccess
                );
            }

            throw new ArtistsFavoritesHttpException(
                (int)HttpStatusCode.InternalServerError,
                FriendlyErrorMessage.GenericInternalAppError
            );
        }
    }
}