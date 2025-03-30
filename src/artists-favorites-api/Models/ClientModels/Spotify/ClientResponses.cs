using System.Text.Json.Serialization;

namespace artists_favorites_api.Models.ClientModels.Spotify 
{
    public record BaiscAccessTokenResponse (
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        [property: JsonPropertyName("expires_in")] int ExpiresIn);

        
}