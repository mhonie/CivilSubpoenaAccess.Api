using Moq;

using CivilSubpoenaAccess.Api.Environment;

namespace CivilSubpoenaAccess.Api.UnitTests.Environment;


public class EnvLoaderTests
{
    private const string invalidEnvFilePath = ".invalid-env";

    [Fact]
    public void Load_FileFound_ShouldSucceed()
    {
        // Arrange
        string envDirectory = AppDomain.CurrentDomain.BaseDirectory;

        string envFilePath = Path.Combine(envDirectory, ".env");

        File.WriteAllText
        (
            envFilePath,

            "CIVIL_SUBPOENA_ACCESS_API_ENVIRONMENT_NAME=Development"
        );

        var envFile = new Mock<IEnvFile>();

        envFile.Setup(x => x.FilePath()).Returns(envFilePath);

        var envLoader = new EnvLoader(envFile.Object);

        // Act
        var loadResult = envLoader.Load();

        // Assert
        Assert.True(loadResult.IsSuccess);
    }

    [Fact]
    public void Load_FileNotFound_ShouldFail()
    {
        // Arrange
        var envFile = new Mock<IEnvFile>();

        envFile.Setup(x => x.FilePath()).Returns(invalidEnvFilePath);

        var envLoader = new EnvLoader(envFile.Object);

        // Act
        var loadResult = envLoader.Load();

        // Assert
        Assert.True(loadResult.IsFailure);

        Assert.NotNull(loadResult.Error);

        Assert.Equal(invalidEnvFilePath, loadResult.Error.FilePath);
    }
}