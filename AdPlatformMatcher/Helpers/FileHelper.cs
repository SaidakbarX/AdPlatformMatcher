using AdPlatformMatcher.Models;

namespace AdPlatformMatcher.Helpers;


    public class FileHelper : IFileHelper
    {
        public async Task<List<Platform>> ParsePlatformsFromFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty");

            using var reader = new StreamReader(file.OpenReadStream());
            var content = await reader.ReadToEndAsync();

            return ParsePlatformsFromText(content);
        }

        
        public List<Platform> ParsePlatformsFromText(string content)
        {
            var platforms = new List<Platform>();

            if (string.IsNullOrWhiteSpace(content))
                return platforms;

            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                try
                {
                    var platform = ParsePlatformLine(line.Trim());
                    if (platform != null)
                    {
                        platforms.Add(platform);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing line '{line}': {ex.Message}");
                }
            }

            return platforms;
        }

       
        private Platform? ParsePlatformLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var colonIndex = line.IndexOf(':');
            if (colonIndex <= 0 || colonIndex >= line.Length - 1)
                return null;

            var platformName = line.Substring(0, colonIndex).Trim();
            var locationsString = line.Substring(colonIndex + 1).Trim();

            if (string.IsNullOrEmpty(platformName) || string.IsNullOrEmpty(locationsString))
                return null;

            var locations = ParseLocations(locationsString);

            if (locations.Count == 0)
                return null;

            return new Platform(platformName, locations);
        }

        private List<string> ParseLocations(string locationsString)
        {
            var locations = new List<string>();

            if (string.IsNullOrWhiteSpace(locationsString))
                return locations;

            var locationParts = locationsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var location in locationParts)
            {
                var trimmedLocation = location.Trim();

                if (!string.IsNullOrEmpty(trimmedLocation) &&
                    trimmedLocation.StartsWith('/') &&
                    trimmedLocation.Length > 1)
                {
                    locations.Add(trimmedLocation);
                }
            }

            return locations;
        }
    }

