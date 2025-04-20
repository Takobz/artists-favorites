namespace artists_favorites_api.Models.DTOs.Responses 
{
    public record GetUserTokenResponseDTO(
        string AccessToken,
        string RefreshToken
    );
}