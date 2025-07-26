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
        var platforms = GetTestPlatforms();

        _platformService.LoadPlatforms(platforms);

        Assert.Equal(4, _platformService.GetPlatformCount());
    }

    [Fact]
    public void GetPlatformsByLocation_ForRuMsk_ShouldReturnCorrectPlatforms()
    {
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        var result = _platformService.GetPlatformsByLocation("/ru/msk");

        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Газета уральских москвичей", result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetPlatformsByLocation_ForRuSvrd_ShouldReturnCorrectPlatforms()
    {
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        var result = _platformService.GetPlatformsByLocation("/ru/svrd");

        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Крутая реклама", result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetPlatformsByLocation_ForRuSvrdRevda_ShouldReturnCorrectPlatforms()
    {
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        var result = _platformService.GetPlatformsByLocation("/ru/svrd/revda");

        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Ревдинский рабочий", result);
        Assert.Contains("Крутая реклама", result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetPlatformsByLocation_ForRu_ShouldReturnOnlyGlobalPlatform()
    {
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        var result = _platformService.GetPlatformsByLocation("/ru");

        Assert.Contains("Яндекс.Директ", result);
        Assert.Single(result);
    }

    [Fact]
    public void GetPlatformsByLocation_ForEmptyLocation_ShouldReturnEmptyList()
    {
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        var result = _platformService.GetPlatformsByLocation("");

        Assert.Empty(result);
    }

    [Fact]
    public void GetPlatformsByLocation_ForNullLocation_ShouldReturnEmptyList()
    {
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        var result = _platformService.GetPlatformsByLocation(null);

        Assert.Empty(result);
    }

    [Fact]
    public void GetPlatformsByLocation_ForNonExistentLocation_ShouldReturnEmptyList()
    {
        var platforms = GetTestPlatforms();
        _platformService.LoadPlatforms(platforms);

        var result = _platformService.GetPlatformsByLocation("/us/ny");

        Assert.Empty(result);
    }

    [Fact]
    public void LoadPlatforms_WithNull_ShouldNotThrow()
    {
        var exception = Record.Exception(() => _platformService.LoadPlatforms(null));
        Assert.Null(exception);
    }

    [Fact]
    public void LoadPlatforms_ShouldReplaceExistingData()
    {
        var initialPlatforms = new List<Platform>
            {
                new("Test Platform", new List<string> { "/test" })
            };

        var newPlatforms = GetTestPlatforms();

        _platformService.LoadPlatforms(initialPlatforms);
        var initialCount = _platformService.GetPlatformCount();

        _platformService.LoadPlatforms(newPlatforms);
        var finalCount = _platformService.GetPlatformCount();

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