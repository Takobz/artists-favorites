using System.Net;
using System.Text.Json;
using artists_favorites_api.Constants;
using artists_favorites_api.Exceptions;
using artists_favorites_api.Models.ClientModels.Spotify;

namespace artists_favorites_api.Clients.Spotify 
{
    public interface ISpotifyTrackClient 
    {
        Task<IEnumerable<SpotifySavedTrack>> GetUserSavedTracks(string accessToken);
    }

    public class SpotifyTrackClient(
        HttpClient httpClient,
        ILogger<SpotifyTrackClient> logger) : ISpotifyTrackClient
    {
        public async Task<IEnumerable<SpotifySavedTrack>> GetUserSavedTracks(string accessToken)
        {
            List<SpotifySavedTrack> allSavedTracks = [];

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            //TODO: use query builder class
            HttpResponseMessage response =  await httpClient.GetAsync("me/tracks?offset=0&limit=50");
            while(response.IsSuccessStatusCode)
            {
                var savedTracks =  await JsonSerializer.DeserializeAsync<SpotifySearchResponse<SpotifySavedTrack>>(
                    await response.Content.ReadAsStreamAsync()
                );

                if (savedTracks == null)
                {
                    logger.LogError("Failed to deserialize Http Content: {HttpResponseContent} to class: {ClassType}",
                        await response.Content.ReadAsStringAsync() ?? string.Empty,
                        nameof(SpotifyCurrentUserProfileResponse)
                    );

                    throw new ArtistsFavoritesHttpException(
                        (int)HttpStatusCode.InternalServerError,
                        FriendlyErrorMessage.GenericInternalAppError
                    );
                }

                allSavedTracks.AddRange(savedTracks.Items);

                if (!string.IsNullOrEmpty(savedTracks.Next)) 
                {
                    response = await httpClient.GetAsync(savedTracks.Next);
                }
                else {
                    return allSavedTracks;
                }
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ArtistsFavoritesHttpException(
                    (int)HttpStatusCode.BadRequest,
                    "Bad Or Expired Token"
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