using artists_favorites_api.Extensions;
using artists_favorites_api.Middleware;
using artists_favorites_api.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSpotifyServices(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSpotifyAuthentication();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //we don't care about CORS for local development.
    app.UseCors(policy => {
        policy.AllowAnyHeader();
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
    });
}
else 
{
    app.UseHttpsRedirection();
    app.UseCors(); //TODO: add CORS policies when in app in live env.
}

app.UseAuthentication();
app.UseAuthorization();
app.MapSpotifyV1Routes();
app.MapSpotifyAuthRoutes();

app.Run();
