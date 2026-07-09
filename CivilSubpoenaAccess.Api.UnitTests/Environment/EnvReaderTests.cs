using System.Collections.Concurrent;

using CivilSubpoenaAccess.Api.Environment;

namespace CivilSubpoenaAccess.Api.UnitTests.Environment;


public class EnvReaderTests
{
    private const string envVariableName = "TEST_ENV_VARIABLE";

    private const string envVariableValue = "testValue";


    [Fact]
    public void ReadValue_CalledTwice_LoadsOnce()
    {
        // Arrange
        System.Environment.SetEnvironmentVariable(envVariableName, envVariableValue);

        var envReader = new EnvReader();

        // Act
        var firstReadResult = envReader.ReadValue(envVariableName);

        var secondReadResult = envReader.ReadValue(envVariableName);

        // Assert
        Assert.True(firstReadResult.IsSuccess);

        Assert.True(secondReadResult.IsSuccess);
    }

    [Fact]
    public void ReadValue_Concurrent_LoadsOnce()
    {
        // Arrange
        System.Environment.SetEnvironmentVariable(envVariableName, envVariableValue);

        var envReader = new EnvReader();

        var readResults = new ConcurrentBag<string>();

        const int numParallelThreads = 10;

        // Act
        Parallel.For(0, numParallelThreads, _ =>
        {
            var readValueResult = envReader.ReadValue(envVariableName);

            if (readValueResult.IsSuccess)

                readResults.Add(readValueResult.Value);
        });

        // Assert
        string firstResult = readResults.First();

        Assert.All(readResults, value => Assert.Equal(firstResult, value));
    }

    [Fact]
    public void ReadValue_Concurrent_ReturnsSameValues()
    {
        // Arrange
        System.Environment.SetEnvironmentVariable(envVariableName, envVariableValue);

        var envReader = new EnvReader();

        var readResults = new ConcurrentBag<string>();

        const int numParallelThreads = 10;

        // Act
        Parallel.For(0, numParallelThreads, _ =>
        {
            var readValueResult = envReader.ReadValue(envVariableName);

            if (readValueResult.IsSuccess)

                readResults.Add(readValueResult.Value);
        });

        // Assert
        Assert.Equal(numParallelThreads, readResults.Count);

        Assert.All(readResults, value => Assert.Equal(envVariableValue, value));
    }

    [Fact]
    public void ReadValue_MissingVariable_ShouldFail()
    {
        // Arrange
        System.Environment.SetEnvironmentVariable(envVariableName, null);
            
        var envReader = new EnvReader();

        // Act
        var readValueResult = envReader.ReadValue(envVariableName);

        // Assert
        Assert.True(readValueResult.IsFailure);

        Assert.NotNull(readValueResult.Error);

        Assert.Equal(envVariableName, readValueResult.Error.VariableName);
    }

    [Fact]
    public void ReadValue_NullVariableName_ShouldFail()
    {
        // Arrange
        System.Environment.SetEnvironmentVariable(envVariableName, envVariableValue);

        var envReader = new EnvReader();

        // Act
        var readValueResult = envReader.ReadValue(null!);

        // Assert
        Assert.True(readValueResult.IsFailure);

        Assert.NotNull(readValueResult.Error);

        Assert.Null(readValueResult.Error.VariableName);
    }
}