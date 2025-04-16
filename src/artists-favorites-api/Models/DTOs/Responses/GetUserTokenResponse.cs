namespace artists_favorites_api.Models.DTOs.Responses 
{
    public record GetUserTokenResponse(
        string AccessToken,
        string RefreshToken
    );
}