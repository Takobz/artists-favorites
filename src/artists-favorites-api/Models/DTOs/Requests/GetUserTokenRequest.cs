namespace artists_favorites_api.Models.DTOs.Requests 
{
    public record GetUserTokenRequest(string AuthorizationCode, string State);
}