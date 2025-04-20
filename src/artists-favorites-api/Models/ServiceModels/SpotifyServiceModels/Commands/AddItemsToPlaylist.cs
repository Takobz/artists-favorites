namespace artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Commands 
{
    public record AddItemsToPlaylistCommand(
        string PlaylistId,
        string AccessToken,
        IEnumerable<string> ItemsToAdd
    );
}