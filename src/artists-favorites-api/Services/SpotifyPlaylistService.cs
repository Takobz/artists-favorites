using artists_favorites_api.Clients.Spotify;
using artists_favorites_api.Extensions;
using artists_favorites_api.Models.ClientModels.Spotify;
using artists_favorites_api.Models.Options;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Commands;
using Microsoft.Extensions.Options;

namespace artists_favorites_api.Services 
{
    public interface ISpotifyPlaylistService 
    {
        Task<CreateEmptyPlaylistResult> CreateEmptyPlaylist(CreateEmptyPlaylistCommand command);
        Task<AddItemsToPlaylistResult> AddItemsToPlaylist(AddItemsToPlaylistCommand command);
    }

    public class SpotifyPlaylistService(
        ISpotifyUserClient spotifyUserClient,
        ISpotifyPlaylistClient spotifyPlaylistClient,
        IOptionsMonitor<SpotifyOptions> options
    ) : ISpotifyPlaylistService
    {
        public async Task<AddItemsToPlaylistResult> AddItemsToPlaylist(AddItemsToPlaylistCommand command)
        {
            var result = await spotifyPlaylistClient.AddTracksToPlaylist(
                command.PlaylistId,
                command.AccessToken,
                new AddItemsToPlaylistRequest(command.ItemsToAdd)
            );

            return result.ToAddItemsToPlaylistResult(
                $"{options.CurrentValue.SpotifyOpenUrl}/playlist/{command.PlaylistId}"
            );
        }

        public async Task<CreateEmptyPlaylistResult> CreateEmptyPlaylist(CreateEmptyPlaylistCommand command)
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