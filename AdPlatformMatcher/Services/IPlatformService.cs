using AdPlatformMatcher.Models;

namespace AdPlatformMatcher.Services;

public interface IPlatformService
{
    void LoadPlatforms(List<Platform> platforms);
    List<string> GetPlatformsByLocation(string location);
   
}
