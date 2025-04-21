namespace artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Commands 
{
    public record AddItemsToPlaylistCommand(
        string PlaylistId,
        IEnumerable<string> ItemsToAdd
    );
}