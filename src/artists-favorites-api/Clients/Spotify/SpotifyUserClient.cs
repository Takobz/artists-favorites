using System.Net;
using System.Text.Json;
using artists_favorites_api.Constants;
using artists_favorites_api.Exceptions;
using artists_favorites_api.Models.ClientModels.Spotify;

namespace artists_favorites_api.Clients.Spotify 
{
    public interface ISpotifyUserClient 
    {
        Task<SpotifyCurrentUserProfileResponse> GetCurrentUserProfileResponse(string accessToken);
    }

    public class SpotifyUserClient(
        HttpClient httpClient,
        ILogger<SpotifyUserClient> logger) : ISpotifyUserClient
    {
        public async Task<SpotifyCurrentUserProfileResponse> GetCurrentUserProfileResponse(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var response = await httpClient.GetAsync(string.Empty);
            if (response.IsSuccessStatusCode) 
            {
                var currentUserProfile =  await JsonSerializer.DeserializeAsync<SpotifyCurrentUserProfileResponse>(
                    await response.Content.ReadAsStreamAsync());
                if (currentUserProfile == null)
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

                return currentUserProfile;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
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



