namespace artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Commands 
{
    public record CreateEmptyPlaylistCommand(
        string PlaylistName,
        string PlaylistDescription,
        bool IsPublicPlaylist
    );
}