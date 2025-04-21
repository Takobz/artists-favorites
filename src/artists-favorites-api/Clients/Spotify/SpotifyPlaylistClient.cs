using artists_favorites_api.Constants;
using artists_favorites_api.Exceptions;
using artists_favorites_api.Extensions;
using artists_favorites_api.Models.ClientModels.Spotify;

namespace artists_favorites_api.Clients.Spotify 
{
    public interface ISpotifyPlaylistClient 
    {
        Task<SpotifyPlaylistResponse> CreatePlaylist(CreatePlaylistRequest request);
        Task<SpotifySnapshotResponse> AddTracksToPlaylist(string playlistId, AddItemsToPlaylistRequest request);
    }

    public class SpotifyPlaylistClient(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        ILogger<SpotifyPlaylistClient> logger) : ISpotifyPlaylistClient
    {
        public async Task<SpotifySnapshotResponse> AddTracksToPlaylist(
            string playlistId,
            AddItemsToPlaylistRequest request
        )
        {
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
            CreatePlaylistRequest request
        )
        {
            if (httpContextAccessor == null || httpContextAccessor?.HttpContext == null)
            {
                throw new ArtistsFavoritesHttpException();
            }

            var userId = httpContextAccessor.HttpContext.User.GetClaimValueByName(
                SpotifyAuthenticationCustomClaims.SpotifyUserEntityId
            );
            
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