using AdPlatformMatcher.Models;
using Xunit;

namespace AdPlatformMatcher.Tests;


public class PlatformModelTests
{
    [Fact]
    public void ServesLocation_WithExactMatch_ShouldReturnTrue()
    {
        var platform = new Platform("Test", new List<string> { "/ru/svrd" });

        var result = platform.ServesLocation("/ru/svrd");

        Assert.True(result);
    }

    [Fact]
    public void ServesLocation_WithSubLocation_ShouldReturnTrue()
    {
        var platform = new Platform("Test", new List<string> { "/ru/svrd" });

        var result = platform.ServesLocation("/ru/svrd/ekb");

        Assert.True(result);
    }

    [Fact]
    public void ServesLocation_WithParentLocation_ShouldReturnFalse()
    {
        var platform = new Platform("Test", new List<string> { "/ru/svrd/ekb" });

        var result = platform.ServesLocation("/ru/svrd");

        Assert.False(result);
    }

    [Fact]
    public void ServesLocation_WithMultipleLocations_ShouldReturnTrueIfAnyMatches()
    {
        var platform = new Platform("Test", new List<string> { "/ru/msk", "/ru/spb", "/ru/svrd" });

        var result1 = platform.ServesLocation("/ru/msk/center");
        var result2 = platform.ServesLocation("/ru/spb");
        var result3 = platform.ServesLocation("/ru/svrd/ekb");
        var result4 = platform.ServesLocation("/ru/nsk");

        Assert.True(result1);
        Assert.True(result2);
        Assert.True(result3);
        Assert.False(result4);
    }

    [Fact]
    public void ServesLocation_WithEmptyLocation_ShouldReturnFalse()
    {
        var platform = new Platform("Test", new List<string> { "/ru/svrd" });

        var result = platform.ServesLocation("");

        Assert.False(result);
    }

    [Fact]
    public void ServesLocation_WithNullLocation_ShouldReturnFalse()
    {
        var platform = new Platform("Test", new List<string> { "/ru/svrd" });

        var result = platform.ServesLocation(null);

        Assert.False(result);
    }

    [Fact]
    public void ServesLocation_WithEmptyPlatformLocations_ShouldReturnFalse()
    {
        var platform = new Platform("Test", new List<string>());

        var result = platform.ServesLocation("/ru/svrd");

        Assert.False(result);
    }

    [Fact]
    public void Constructor_WithNullLocations_ShouldInitializeEmptyList()
    {
        var platform = new Platform("Test", null);

        Assert.NotNull(platform.Locations);
        Assert.Empty(platform.Locations);
    }

    [Fact]
    public void ToString_ShouldReturnCorrectFormat()
    {
        var platform = new Platform("Test Platform", new List<string> { "/ru/msk", "/ru/spb" });

        var result = platform.ToString();

        Assert.Equal("Test Platform: /ru/msk,/ru/spb", result);
    }

    [Fact]
    public void ServesLocation_CaseInsensitive_ShouldWork()
    {
        var platform = new Platform("Test", new List<string> { "/RU/SVRD" });

        var result = platform.ServesLocation("/ru/svrd/ekb");

        Assert.True(result);
    }
}