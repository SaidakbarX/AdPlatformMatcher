namespace AdPlatformMatcher.Models;

public class Platform
{
    public string Name { get; set; } = string.Empty;
    public List<string> Locations { get; set; } = new();

    public Platform() { }

    public Platform(string name, List<string> locations)
    {
        Name = name;
        Locations = locations ?? new List<string>();
    }


    public bool ServesLocation(string targetLocation)
    {
        if (string.IsNullOrEmpty(targetLocation))
            return false;

        targetLocation = targetLocation.TrimEnd('/').ToLowerInvariant();

        return Locations.Any(location =>
            targetLocation.StartsWith(location.TrimEnd('/').ToLowerInvariant()));
    }
    public override string ToString()
    {
        return $"{Name}: {string.Join(",", Locations)}";
    }
}
