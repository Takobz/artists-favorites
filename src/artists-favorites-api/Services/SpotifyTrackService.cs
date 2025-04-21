using artists_favorites_api.Clients.Spotify;
using artists_favorites_api.Extensions;
using artists_favorites_api.Models.ClientModels.Spotify;
using artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries;

namespace artists_favorites_api.Services 
{
    public interface ISpotifyTrackService 
    {
        Task<IEnumerable<GetSavedTrackResult>> GetUserSavedTracks(GetSavedTracksQuery query);
    }

    public class SpotifyTrackService(
        ISpotifyTrackClient spotifyTrackClient
        ) : ISpotifyTrackService
    {
        public async Task<IEnumerable<GetSavedTrackResult>> GetUserSavedTracks(GetSavedTracksQuery query)
        {
            var allSavedTracks = await spotifyTrackClient.GetUserSavedTracks();
            return allSavedTracks
                .Where(st => st.Track.Artists.Any(a => ArtistIdIsList(query.ArtistEntityId, a)))
                .Select(st => st.ToSavedTrackResult());
        }

        private static bool ArtistIdIsList(string artistEntityId, SpotifySimplifiedArtist artist)
        {
            return artist.EntityId.Equals(artistEntityId);
        }
    }
}