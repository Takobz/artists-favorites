using artists_favorites_api.Clients.Spotify;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Commands;

namespace artists_favorites_api.Services 
{
    public interface ISpotifyPlaylistService 
    {
        Task<CreateEmptyPlaylistResult> CreateEmptyPlaylist(CreateEmptyPlaylist command);
    }

    public class SpotifyPlaylistService(
        ISpotifyUserClient spotifyUserClient,
        ISpotifyPlaylistClient spotifyPlaylistClient
    ) : ISpotifyPlaylistService
    {
        public async Task<CreateEmptyPlaylistResult> CreateEmptyPlaylist(CreateEmptyPlaylist command)
        {
            var userProfile = await spotifyUserClient.GetCurrentUserProfileResponse(command.AccessToken);

            return new CreateEmptyPlaylistResult(
                string.Empty,
                string.Empty,
                string.Empty
            );
        }
    }
}