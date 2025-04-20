namespace artists_favorites_api.Models.DTOs.Responses 
{
    public record ErrorResponseDTO(
        int HttpStatusCode,
        string Message
    );
}