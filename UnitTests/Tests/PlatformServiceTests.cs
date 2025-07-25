using AdPlatformMatcher.Models;
using AdPlatformMatcher.Services;
using Xunit;

namespace AdPlatformMatcher.Tests;

public class PlatformServiceTests
{
    private readonly IPlatformService _platformService;

    public PlatformServiceTests()
    {
        _platformService = new PlatformService();
    }

    [Fact]
    public void LoadPlatforms_ShouldStoreCorrectCount()
    {
        // Arrange
        var platforms = GetTestPlatforms();

        // Act
        _platformService.LoadPlatforms(platforms);

        // Assert
        Assert.Equal(4, _platformService.GetPlatformCount());
    }

    [Fact]
    public void GetPlatformsByLocation_ForRuMsk_ShouldReturnCorrectPlatforms()
    {
        // Arrange
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        // Act
        var result = _platformService.GetPlatformsByLocation("/ru/msk");

        // Assert
        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Газета уральских москвичей", result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetPlatformsByLocation_ForRuSvrd_ShouldReturnCorrectPlatforms()
    {
        // Arrange
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        // Act
        var result = _platformService.GetPlatformsByLocation("/ru/svrd");

        // Assert
        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Крутая реклама", result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetPlatformsByLocation_ForRuSvrdRevda_ShouldReturnCorrectPlatforms()
    {
        // Arrange
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        // Act
        var result = _platformService.GetPlatformsByLocation("/ru/svrd/revda");

        // Assert
        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Ревдинский рабочий", result);
        Assert.Contains("Крутая реклама", result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetPlatformsByLocation_ForRu_ShouldReturnOnlyGlobalPlatform()
    {
        // Arrange
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        // Act
        var result = _platformService.GetPlatformsByLocation("/ru");

        // Assert
        Assert.Contains("Яндекс.Директ", result);
        Assert.Single(result);
    }

    [Fact]
    public void GetPlatformsByLocation_ForEmptyLocation_ShouldReturnEmptyList()
    {
        // Arrange
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        // Act
        var result = _platformService.GetPlatformsByLocation("");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetPlatformsByLocation_ForNullLocation_ShouldReturnEmptyList()
    {
        // Arrange
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        // Act
        var result = _platformService.GetPlatformsByLocation(null);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetPlatformsByLocation_ForNonExistentLocation_ShouldReturnEmptyList()
    {
        // Arrange
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        // Act
        var result = _platformService.GetPlatformsByLocation("/us/ny");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void LoadPlatforms_WithNull_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _platformService.LoadPlatforms(null));
        Assert.Null(exception);
    }

    [Fact]
    public void LoadPlatforms_ShouldReplaceExistingData()
    {
        // Arrange
        var initialPlatforms = new List<Platform>
            {
                new("Test Platform", new List<string> { "/test" })
            };

        var newPlatforms = GetTestPlatforms();

        // Act
        _platformService.LoadPlatforms(initialPlatforms);
        var initialCount = _platformService.GetPlatformCount();

        _platformService.LoadPlatforms(newPlatforms);
        var finalCount = _platformService.GetPlatformCount();

        // Assert
        Assert.Equal(1, initialCount);
        Assert.Equal(4, finalCount);
    }

    private List<Platform> GetTestPlatforms()
    {
        return new List<Platform>
            {
                new("Яндекс.Директ", new List<string> { "/ru" }),
                new("Ревдинский рабочий", new List<string> { "/ru/svrd/revda", "/ru/svrd/pervik" }),
                new("Газета уральских москвичей", new List<string> { "/ru/msk", "/ru/permobl", "/ru/chelobl" }),
                new("Крутая реклама", new List<string> { "/ru/svrd" })
            };
    }
}