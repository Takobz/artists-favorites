namespace artists_favorites_api.Models.DTOs.Requests 
{
    public record CreatePlaylistRequest(
    string PlaylistName,
    string PlaylistDescription,
    bool IsPublicPlaylist);
}