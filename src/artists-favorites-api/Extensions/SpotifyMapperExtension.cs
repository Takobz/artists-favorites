using artists_favorites_api.Models.ClientModels.Spotify;
using artists_favorites_api.Models.DTOs.Requests;
using artists_favorites_api.Models.DTOs.Responses;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Commands;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries;

namespace artists_favorites_api.Extensions 
{
    public static class SpotifyMapperExtension 
    {
        public static IEnumerable<SearchArtistResult> ResultsFromSearchResponse(this SpotifySearchResponses searchResponses)
        {
            if (!searchResponses.ArtistsSearchResponses.Items.Any()) return [];

            return searchResponses.ArtistsSearchResponses.Items.Select(item  => 
            {
                return new SearchArtistResult(
                    item.ArtistName,
                    item.ExternalUrls.SpotifyUrl,
                    item.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                    item.ArtistPopularity,
                    item.EntityId
                );
            });
        }

        public static SearchArtistResponseDTO ToSearchArtistResponseDTO (this SearchArtistResult result)
        {
            return new SearchArtistResponseDTO(
                result.ArtistName,
                result.ArtistSpotifyUrl,
                result.ArtistImageUrl,
                result.ArtistPopularity,
                result.ArtistEntityId);
        }

        public static bool SearchFoundNoArtists (this SpotifySearchResponses? searchResponses){
            return 
                searchResponses == null ||
                searchResponses.ArtistsSearchResponses == null ||
                searchResponses.ArtistsSearchResponses.Items == null ||
                !searchResponses.ArtistsSearchResponses.Items.Any();
        }

        public static GetUserTokenResponseDTO ToGetUserTokenResponseDTO(this AuthorizationCodeAccessTokenResponse? response)
        {
            if (response == null) throw new Exception("Can't convert a null response");

            return new GetUserTokenResponseDTO(
                response.AccessToken,
                response.RefreshToken
            );
        }

        public static CreateEmptyPlaylistCommand ToCreateEmptyPlaylistCommand(this Models.DTOs.Requests.CreatePlaylistRequestDTO request, string accessToken)
        {
            return new CreateEmptyPlaylistCommand(
                request.PlaylistName,
                request.PlaylistDescription,
                request.IsPublicPlaylist,
                accessToken
            );
        }

        public static CreatePlaylistRequest ToCreatePlaylistClientModel(this CreateEmptyPlaylistCommand request) 
        {
            return new CreatePlaylistRequest(
                request.PlaylistName,
                request.PlaylistDescription,
                request.IsPublicPlaylist
            );
        }

        public static CreateEmptyPlaylistResult ToCreateEmptyPlaylistResult(this SpotifyPlaylistResponse response)
        {
            return new CreateEmptyPlaylistResult(
                response.EntityId,
                response.Name,
                response.Description
            );
        }

        public static GetSavedTrackResult ToSavedTrackResult(this SpotifySavedTrack response)
        {
            return new GetSavedTrackResult(
                response.Track.EntityId,
                response.Track.TrackName,
                response.Track.Album.Images.FirstOrDefault()?.ImageUrl ?? string.Empty
            );
        }

        public static AddItemsToPlaylistResult ToAddItemsToPlaylistResult(
            this SpotifySnapshotResponse response,
            string playlistUri
        )
        {
            return new AddItemsToPlaylistResult(
                response.SnapshotId,
                playlistUri
            );
        }

        public static AddItemsToPlaylistCommand ToAddItemsCommand(
            this AddItemsToPlaylistRequestDTO dto,
            string playlistId,
            string accessToken
        )
        {   List<string> spotifyUris = [];
            if (dto.Tracks.Any())
            {
                spotifyUris.AddRange(
                    dto.Tracks.Select(id => $"spotify:track:{id}")
                );
            }

            if (dto.Shows.Any()) 
            {
                spotifyUris.AddRange(
                    dto.Shows.Select(id => $"spotify:show:{id}")
                );
            }

            return new AddItemsToPlaylistCommand(
                playlistId,
                accessToken,
                spotifyUris
            );
        }
    }
}