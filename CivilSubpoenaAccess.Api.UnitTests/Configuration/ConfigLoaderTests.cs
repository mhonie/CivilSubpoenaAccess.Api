using Microsoft.Extensions.Configuration;
using Moq;

using CivilSubpoenaAccess.Api.Configuration;


namespace CivilSubpoenaAccess.Api.UnitTests.Configuration;


public class ConfigLoaderTests
{
    private const string appSettingsFileName = "appSettings.json";

    private static ConfigLoader CreateConfigLoader
    (
        IConfigurationBuilder? configBuilder = null
    )
    {
        var configFile = new Mock<IConfigFile>();

        configFile.Setup(x => x.Directory).Returns(AppDomain.CurrentDomain.BaseDirectory);

        configFile.Setup(x => x.DefaultFileName).Returns(appSettingsFileName);

        configFile.Setup(x => x.FileName).Returns(appSettingsFileName);

        configBuilder ??= new ConfigurationBuilder();

        var configLoader = new ConfigLoader(configFile.Object, configBuilder);

        return configLoader;
    }

    [Fact]
    public void Load_ValidConfiguration_ShouldSucceed()
    {
        // Arrange
        var configLoader = CreateConfigLoader();

        // Act
        var loadResult = configLoader.Load();

        // Assert
        Assert.True(loadResult.IsSuccess);

        Assert.NotNull(loadResult.Value);
    }

    [Fact]
    public void Load_MultipleCalls_ShouldReturnSameConfiguration()
    {
        // Arrange
        var configLoader = CreateConfigLoader();

        // Act
        var firstLoadResult = configLoader.Load();

        var secondLoadResult = configLoader.Load();

        // Assert
        Assert.True(firstLoadResult.IsSuccess);

        Assert.True(secondLoadResult.IsSuccess);

        Assert.Same(firstLoadResult.Value, secondLoadResult.Value);
    }

    [Fact]
    public void Load_InvalidDirectory_ShouldFail()
    {
        // Arrange
        var configFile = new Mock<IConfigFile>();

        configFile.Setup(x => x.Directory)

            .Returns("Z:\\this-directory-does-not-exist");

        configFile.Setup(x => x.DefaultFileName).Returns(appSettingsFileName);

        configFile.Setup(x => x.FileName).Returns(appSettingsFileName);

        IConfigurationBuilder configBuilder = new ConfigurationBuilder();

        var configLoader = new ConfigLoader(configFile.Object, configBuilder);

        // Act
        var loadResult = configLoader.Load();

        // Assert
        Assert.True(loadResult.IsFailure);

        Assert.NotNull(loadResult.Error);

        Assert.NotNull(loadResult.Error.Exception);

        Assert.Equal(appSettingsFileName, loadResult.Error.FileName);
    }
}