namespace artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Queries 
{
    public record SearchArtistResult(
        string ArtistName,
        string ArtistSpotifyUrl,
        string ArtistImageUrl,
        int ArtistPopularity,
        string ArtistEntityId
    );
}