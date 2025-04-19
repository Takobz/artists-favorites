namespace artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries 
{
    public record GetSavedTrackResult(
        string TrackId,
        string TrackName,
        string TrackAlbumImage = ""
    );
}