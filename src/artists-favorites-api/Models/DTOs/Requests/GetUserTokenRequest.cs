namespace artists_favorites_api.Models.DTOs.Requests 
{
    public record GetUserTokenRequestDTO(string AuthorizationCode, string State);
}