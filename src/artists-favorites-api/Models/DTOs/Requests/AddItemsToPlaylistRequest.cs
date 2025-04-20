namespace artists_favorites_api.Models.DTOs.Requests 
{
    public record AddItemsToPlaylistRequestDTO(
        IEnumerable<string> Tracks,
        IEnumerable<string> Shows
    );
}