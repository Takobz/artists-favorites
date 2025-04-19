namespace artists_favorites_api.Models.DTOs.Responses 
{
    public record SearchArtistResponse(
        string Name,
        string SpotifyUrl,
        string ImageUrl,
        int ArtistPopularity,
        string ArtistEntityId);
}