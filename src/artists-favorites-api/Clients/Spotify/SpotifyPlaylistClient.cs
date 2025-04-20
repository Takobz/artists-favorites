using artists_favorites_api.Extensions;
using artists_favorites_api.Models.ClientModels.Spotify;

namespace artists_favorites_api.Clients.Spotify 
{
    public interface ISpotifyPlaylistClient 
    {
        Task<SpotifyPlaylistResponse> CreatePlaylist(string userId, string accessToken, CreatePlaylistRequest request);
        Task<SpotifySnapshotResponse> AddTracksToPlaylist(string playlistId, string accessToken, AddItemsToPlaylistRequest request);
    }

    public class SpotifyPlaylistClient(
        HttpClient httpClient,
        ILogger<SpotifyPlaylistClient> logger) : ISpotifyPlaylistClient
    {
        public async Task<SpotifySnapshotResponse> AddTracksToPlaylist(
            string playlistId,
            string accessToken, 
            AddItemsToPlaylistRequest request
        )
        {
            //TODO: move to delegating handler that will look for access token in claim principle.
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var httpRequestMessage = new HttpRequestMessage 
            {
                Content = JsonContent.Create(request),
                RequestUri = new Uri($"v1/playlists/{playlistId}/tracks", UriKind.Relative),
                Method = HttpMethod.Post
            };

            return await httpClient
                .SpotifySendAsyncAndReturnResponse<SpotifySnapshotResponse, SpotifyPlaylistClient>(httpRequestMessage, logger);
        }

        public async Task<SpotifyPlaylistResponse> CreatePlaylist(
            string userId,
            string accessToken,
            CreatePlaylistRequest request
        )
        {
            //TODO: move to delegating handler that will look for access token in claim principle.
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var httpRequestMessage = new HttpRequestMessage 
            {
                Content = JsonContent.Create(request),
                RequestUri = new Uri($"v1/users/{userId}/playlists", UriKind.Relative),
                Method = HttpMethod.Post
            };

            return await httpClient
                .SpotifySendAsyncAndReturnResponse<SpotifyPlaylistResponse, SpotifyPlaylistClient>(httpRequestMessage, logger);
        }
    }
}