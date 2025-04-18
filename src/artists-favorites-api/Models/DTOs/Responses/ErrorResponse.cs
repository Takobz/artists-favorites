namespace artists_favorites_api.Models.DTOs.Responses 
{
    public record ErrorResponse(
        int HttpStatusCode,
        string Message
    );
}