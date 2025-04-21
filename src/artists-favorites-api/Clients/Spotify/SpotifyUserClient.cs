using artists_favorites_api.Models.ClientModels.Spotify;
using artists_favorites_api.Extensions;

namespace artists_favorites_api.Clients.Spotify 
{
    public interface ISpotifyUserClient 
    {
        Task<SpotifyCurrentUserProfileResponse> GetCurrentUserProfileResponse(string accessToken);
    }

    //Refactor: Make this an auth provider service method because this is also used in an Auth Handler
    public class SpotifyUserClient(
        HttpClient httpClient,
        ILogger<SpotifyUserClient> logger) : ISpotifyUserClient
    {
        public async Task<SpotifyCurrentUserProfileResponse> GetCurrentUserProfileResponse(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            return await httpClient
                .SpotifySendAsyncAndReturnResponse<SpotifyCurrentUserProfileResponse, SpotifyUserClient>(
                    new HttpRequestMessage {
                        Method = HttpMethod.Get
                    },
                    logger
                );
        }
    }
}



