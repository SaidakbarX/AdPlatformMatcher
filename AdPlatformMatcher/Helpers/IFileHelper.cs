using AdPlatformMatcher.Models;

namespace AdPlatformMatcher.Helpers;

public interface IFileHelper
{
    Task<List<Platform>> ParsePlatformsFromFileAsync(IFormFile file);
    List<Platform> ParsePlatformsFromText(string content);
}