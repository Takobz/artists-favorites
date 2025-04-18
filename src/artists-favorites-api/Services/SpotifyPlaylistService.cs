using artists_favorites_api.Clients.Spotify;
using artists_favorites_api.Extensions;
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
            var request = command.ToCreatePlaylistClientModel();
            var userProfile = await spotifyUserClient.GetCurrentUserProfileResponse(command.AccessToken);
            var createPlaylist = await spotifyPlaylistClient.CreatePlaylist(
                userProfile.EntityId,
                command.AccessToken,
                request);

            return createPlaylist.ToCreateEmptyPlaylistResult();
        }
    }
}