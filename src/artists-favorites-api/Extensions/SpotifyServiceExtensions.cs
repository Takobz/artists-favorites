using artists_favorites_api.Models.ClientModels.Spotify;
using artists_favorites_api.Models.DTOs.Requests;
using artists_favorites_api.Models.DTOs.Responses;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Commands;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries;

namespace artists_favorites_api.Extensions 
{
    public static class SpotifyServiceExtensions 
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
                    item.ArtistPopularity
                );
            });
        }

        public static SearchArtistResponse ToSearchArtistResponseDTO (this SearchArtistResult result)
        {
            return new SearchArtistResponse(
                result.ArtistName,
                result.ArtistSpotifyUrl,
                result.ArtistImageUrl,
                result.ArtistPopularity);
        }

        public static bool SearchFoundNoArtists (this SpotifySearchResponses? searchResponses){
            return 
                searchResponses == null ||
                searchResponses.ArtistsSearchResponses == null ||
                searchResponses.ArtistsSearchResponses.Items == null ||
                !searchResponses.ArtistsSearchResponses.Items.Any();
        }

        public static GetUserTokenResponse ToGetUserTokenResponseDTO(this AuthorizationCodeAccessTokenResponse? response)
        {
            if (response == null) throw new Exception("Can't convert a null response");

            return new GetUserTokenResponse(
                response.AccessToken,
                response.RefreshToken
            );
        }

        public static CreateEmptyPlaylist ToCreateEmptyPlaylistCommand(this CreatePlaylistRequest request, string accessToken)
        {
            return new CreateEmptyPlaylist(
                request.PlaylistName,
                request.PlaylistDescription,
                request.IsPublicPlaylist,
                accessToken
            );
        }

        public static CreatePlaylist ToCreatePlaylistClientModel(this CreateEmptyPlaylist request) 
        {
            return new CreatePlaylist(
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
    }
}