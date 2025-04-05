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
}