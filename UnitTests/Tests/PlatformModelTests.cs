using AdPlatformMatcher.Models;
using Xunit;

namespace AdPlatformMatcher.Tests;


public class PlatformModelTests
{
    [Fact]
    public void ServesLocation_WithExactMatch_ShouldReturnTrue()
    {
        // Arrange
        var platform = new Platform("Test", new List<string> { "/ru/svrd" });

        // Act
        var result = platform.ServesLocation("/ru/svrd");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ServesLocation_WithSubLocation_ShouldReturnTrue()
    {
        // Arrange
        var platform = new Platform("Test", new List<string> { "/ru/svrd" });

        // Act
        var result = platform.ServesLocation("/ru/svrd/ekb");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ServesLocation_WithParentLocation_ShouldReturnFalse()
    {
        // Arrange
        var platform = new Platform("Test", new List<string> { "/ru/svrd/ekb" });

        // Act
        var result = platform.ServesLocation("/ru/svrd");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ServesLocation_WithMultipleLocations_ShouldReturnTrueIfAnyMatches()
    {
        // Arrange
        var platform = new Platform("Test", new List<string> { "/ru/msk", "/ru/spb", "/ru/svrd" });

        // Act
        var result1 = platform.ServesLocation("/ru/msk/center");
        var result2 = platform.ServesLocation("/ru/spb");
        var result3 = platform.ServesLocation("/ru/svrd/ekb");
        var result4 = platform.ServesLocation("/ru/nsk");

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.True(result3);
        Assert.False(result4);
    }

    [Fact]
    public void ServesLocation_WithEmptyLocation_ShouldReturnFalse()
    {
        // Arrange
        var platform = new Platform("Test", new List<string> { "/ru/svrd" });

        // Act
        var result = platform.ServesLocation("");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ServesLocation_WithNullLocation_ShouldReturnFalse()
    {
        // Arrange
        var platform = new Platform("Test", new List<string> { "/ru/svrd" });

        // Act
        var result = platform.ServesLocation(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ServesLocation_WithEmptyPlatformLocations_ShouldReturnFalse()
    {
        // Arrange
        var platform = new Platform("Test", new List<string>());

        // Act
        var result = platform.ServesLocation("/ru/svrd");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Constructor_WithNullLocations_ShouldInitializeEmptyList()
    {
        // Act
        var platform = new Platform("Test", null);

        // Assert
        Assert.NotNull(platform.Locations);
        Assert.Empty(platform.Locations);
    }

    [Fact]
    public void ToString_ShouldReturnCorrectFormat()
    {
        // Arrange
        var platform = new Platform("Test Platform", new List<string> { "/ru/msk", "/ru/spb" });

        // Act
        var result = platform.ToString();

        // Assert
        Assert.Equal("Test Platform: /ru/msk,/ru/spb", result);
    }

    [Fact]
    public void ServesLocation_CaseInsensitive_ShouldWork()
    {
        // Arrange
        var platform = new Platform("Test", new List<string> { "/RU/SVRD" });

        // Act
        var result = platform.ServesLocation("/ru/svrd/ekb");

        // Assert
        Assert.True(result);
    }
}