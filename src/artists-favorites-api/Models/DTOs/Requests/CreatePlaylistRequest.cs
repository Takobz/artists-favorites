namespace artists_favorites_api.Models.DTOs.Requests 
{
    public record CreatePlaylistRequestDTO(
    string PlaylistName,
    string PlaylistDescription,
    bool IsPublicPlaylist);
}