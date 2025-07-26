using AdPlatformMatcher.Helpers;
using AdPlatformMatcher.Models;

namespace AdPlatformMatcher.Services;

public class PlatformService:IPlatformService
{
    private readonly object _lock = new();
    private List<Platform> _platforms;

    public PlatformService()
    {
        if(_platforms == null)
        {
            _platforms = new List<Platform>();
        }
    }

    private Dictionary<string, List<string>> _locationIndex = new();

    public void LoadPlatforms(List<Platform> platforms)
    {
        lock (_lock)
        {
            _platforms = platforms ?? new List<Platform>();
            RebuildLocationIndex();
        }
    }

    public List<string> GetPlatformsByLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return new List<string>();

        lock (_lock)
        {
            var locations = location
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.TrimEnd('/').ToLowerInvariant())
                .ToList();

            var result = new List<string>();

            foreach (var platform in _platforms)
            {
                if (locations.All(loc => platform.ServesLocation(loc)))
                {
                    result.Add(platform.Name);
                }
            }

            return result;
        }
    }

    
    private void RebuildLocationIndex()
    {
        _locationIndex.Clear();

        foreach (var platform in _platforms)
        {
            foreach (var location in platform.Locations)
            {
                var normalizedLocation = location.TrimEnd('/');

                if (!_locationIndex.ContainsKey(normalizedLocation))
                {
                    _locationIndex[normalizedLocation] = new List<string>();
                }

                if (!_locationIndex[normalizedLocation].Contains(platform.Name))
                {
                    _locationIndex[normalizedLocation].Add(platform.Name);
                }
            }
        }
    }
}
