using AdPlatformMatcher.Helpers;
using AdPlatformMatcher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AdPlatformMatcher.Controllers;


public static class AdPlatformEndpoints
{
    public static void MapAdPlatformEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/platforms/upload", async (IFormFile file, IPlatformService platformService, IFileHelper fileHelper) =>
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Results.BadRequest(new { error = "File is required" });
                }
                if (!file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    return Results.BadRequest(new { error = "Only .txt files are allowed" });
                }
                var platforms = await fileHelper.ParsePlatformsFromFileAsync(file);
                platformService.LoadPlatforms(platforms);
                return Results.Ok(new { message = "Platforms loaded successfully", count = platforms.Count });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Error processing file: {ex.Message}");
            }
        })
        .WithName("UploadPlatforms")
        .WithTags("Platforms")
        .DisableAntiforgery();
        app.MapGet("/api/platforms/search", (string location, IPlatformService platformService) =>
        {
            try
            {
                if (string.IsNullOrWhiteSpace(location))
                {
                    return Results.BadRequest(new { error = "Location parameter is required" });
                }
                var platforms = platformService.GetPlatformsByLocation(location);
                return Results.Ok(new { location, platforms });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Error searching platforms: {ex.Message}");
            }
        })
        .WithName("SearchPlatforms")
        .WithTags("Platforms")
        .DisableAntiforgery();
    }
}
