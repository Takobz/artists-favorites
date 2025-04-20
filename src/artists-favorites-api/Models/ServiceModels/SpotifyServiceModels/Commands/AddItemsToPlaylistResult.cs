namespace artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Commands 
{
    public record AddItemsToPlaylistResult(
        string SnapshotId,
        string PlaylistUri
    );
}