namespace artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Commands 
{
    public record CreateEmptyPlaylistResult(
        string PlaylistId,
        string PlaylistName,
        string PlaylistDescription
    );
}