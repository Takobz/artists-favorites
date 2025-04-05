using artists_favorites_api.Models.ClientModels.Spotify;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries;

namespace artists_favorites_api.Extensions 
{
    public static class SpotifyServiceResultsExtensions 
    {
        public static IEnumerable<SearchArtistResult> ResultsFromSearchResponse(this SpotifySearchResponses searchResponses)
        {
            if (!searchResponses.ArtistsSearchResponses.Items.Any()) return [];

            return searchResponses.ArtistsSearchResponses.Items.Select(item  => 
            {
                return new SearchArtistResult(
                    item.ArtistName,
                    item.ExternalUrls.SpotifyUrl,
                    item.Images.FirstOrDefault()?.ImageUrl ?? string.Empty
                );
            });
        }

        public static bool SearchFoundNoArtists (this SpotifySearchResponses? searchResponses){
            return 
                searchResponses == null ||
                searchResponses.ArtistsSearchResponses == null ||
                searchResponses.ArtistsSearchResponses.Items == null ||
                !searchResponses.ArtistsSearchResponses.Items.Any();
        }
    }
}