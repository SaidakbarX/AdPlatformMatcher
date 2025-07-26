using AdPlatformMatcher.Helpers;
using Xunit;

namespace AdPlatformMatcher.Tests;

public class FileHelperTests
{
    private readonly IFileHelper _fileHelper;

    public FileHelperTests()
    {
        _fileHelper = new FileHelper();
    }

    [Fact]
    public void ParsePlatformsFromText_WithValidContent_ShouldReturnCorrectPlatforms()
    {
        var content = @"Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
Крутая реклама:/ru/svrd";

        var result = _fileHelper.ParsePlatformsFromText(content);

        Assert.Equal(4, result.Count);

        var yandex = result.First(p => p.Name == "Яндекс.Директ");
        Assert.Single(yandex.Locations);
        Assert.Contains("/ru", yandex.Locations);

        var revda = result.First(p => p.Name == "Ревдинский рабочий");
        Assert.Equal(2, revda.Locations.Count);
        Assert.Contains("/ru/svrd/revda", revda.Locations);
        Assert.Contains("/ru/svrd/pervik", revda.Locations);

        var gazeta = result.First(p => p.Name == "Газета уральских москвичей");
        Assert.Equal(3, gazeta.Locations.Count);
        Assert.Contains("/ru/msk", gazeta.Locations);
        Assert.Contains("/ru/permobl", gazeta.Locations);
        Assert.Contains("/ru/chelobl", gazeta.Locations);
    }

    [Fact]
    public void ParsePlatformsFromText_WithEmptyContent_ShouldReturnEmptyList()
    {
        var result = _fileHelper.ParsePlatformsFromText("");

        Assert.Empty(result);
    }

    [Fact]
    public void ParsePlatformsFromText_WithNullContent_ShouldReturnEmptyList()
    {
        var result = _fileHelper.ParsePlatformsFromText(null);

        Assert.Empty(result);
    }

    [Fact]
    public void ParsePlatformsFromText_WithWhitespaceContent_ShouldReturnEmptyList()
    {
        var result = _fileHelper.ParsePlatformsFromText("   \n\r\t   ");

        Assert.Empty(result);
    }

    [Fact]
    public void ParsePlatformsFromText_WithInvalidLines_ShouldSkipInvalidLines()
    {
        var content = @"Valid Platform:/ru/test
    Invalid Line Without Colon
   :/ru/test2
    Platform Without Location:
     :
  Platform With Empty Location:,,,
   Valid Platform 2:/ru/test3";

        var result = _fileHelper.ParsePlatformsFromText(content);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "Valid Platform");
        Assert.Contains(result, p => p.Name == "Valid Platform 2");
    }

    [Fact]
    public void ParsePlatformsFromText_WithInvalidLocations_ShouldSkipInvalidLocations()
    {
        var content = "Test Platform:/ru/valid,invalid-location,/ru/valid2,not-starting-with-slash,/";

        var result = _fileHelper.ParsePlatformsFromText(content);

        Assert.Single(result);
        var platform = result.First();
        Assert.Equal(2, platform.Locations.Count);
        Assert.Contains("/ru/valid", platform.Locations);
        Assert.Contains("/ru/valid2", platform.Locations);
    }

    [Fact]
    public void ParsePlatformsFromText_WithExtraWhitespace_ShouldTrimCorrectly()
    {
        var content = @"  Platform With Spaces  :  /ru/test1  ,  /ru/test2  
   Another Platform   :   /ru/test3   ";

        var result = _fileHelper.ParsePlatformsFromText(content);

        Assert.Equal(2, result.Count);

        var platform1 = result.First(p => p.Name == "Platform With Spaces");
        Assert.Equal(2, platform1.Locations.Count);
        Assert.Contains("/ru/test1", platform1.Locations);
        Assert.Contains("/ru/test2", platform1.Locations);

        var platform2 = result.First(p => p.Name == "Another Platform");
        Assert.Single(platform2.Locations);
        Assert.Contains("/ru/test3", platform2.Locations);
    }

    [Fact]
    public void ParsePlatformsFromText_WithDifferentLineEndings_ShouldParseCorrectly()
    {
        var content = "Platform1:/ru/test1\nPlatform2:/ru/test2\r\nPlatform3:/ru/test3\rPlatform4:/ru/test4";

        var result = _fileHelper.ParsePlatformsFromText(content);

        Assert.Equal(4, result.Count);
        Assert.Contains(result, p => p.Name == "Platform1");
        Assert.Contains(result, p => p.Name == "Platform2");
        Assert.Contains(result, p => p.Name == "Platform3");
        Assert.Contains(result, p => p.Name == "Platform4");
    }
}