using System.Text;
using artists_favorites_api.Models.ClientModels.Spotify;

namespace artists_favorites_api.Helpers 
{
    public static class SpotifyV1QueryBuilder 
    {
        private static readonly List<string> _searchTypes = [
            "album",
            "artist",
            "playlist",
            "track",
            "show",
            "episode"
        ];

        public static string SearchAlbum(this string albumName) 
        {
            return $"?q=album:{albumName}&type={_searchTypes[0]}";
        }

        public static string SearchArtist(this string artistName) 
        {
            return $"?q=artist:{artistName}&type={_searchTypes[1]}";
        }
    }

    public static class UrlQueryParamsBuilder
    {
        public static string ToAuthorizeQueryParams(this AuthorizeUserRequest request)
        {
            return
                $"client_id={request.ClientId}&" +
                $"response_type={request.ResponseType}&" +
                $"scope={request.Scope}&" +
                $"redirect_uri={request.RedirectUri}&" +
                $"state={request.State}";
        }
    }
}