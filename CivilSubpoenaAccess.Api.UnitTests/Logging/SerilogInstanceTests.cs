using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Concurrent;

using CivilSubpoenaAccess.Api.Logging;

namespace CivilSubpoenaAccess.Api.UnitTests.Logging;


public class SerilogInstanceTests
{
    private static SerilogInstance CreateSerilogInstance()
    {
        var configuration = new ConfigurationBuilder()

            .AddInMemoryCollection([])

            .Build();

        var serilogInstance = new SerilogInstance(configuration);

        return serilogInstance;
    }

    [Fact]
    public void GetInstance_CalledTwice_ReturnsSameInstance()
    {
        // Arrange
        var serilogInstance = CreateSerilogInstance();

        // Act
        var firstLogger = serilogInstance.Get();

        var secondLogger = serilogInstance.Get();

        // Assert
        Assert.True(firstLogger.IsSuccess);

        Assert.True(secondLogger.IsSuccess);

        Assert.Same(firstLogger.Value, secondLogger.Value);
    }

    [Fact]
    public void GetInstance_Concurrent_CreatesOnce()
    {
        // Arrange
        var serilogInstance = CreateSerilogInstance();

        var seriloggers = new ConcurrentBag<ILogger>();

        const int numParallelThreads = 10;

        // Act
        Parallel.For(0, numParallelThreads, _ =>
        {
            var seriloggerResult = serilogInstance.Get();

            if (seriloggerResult.IsSuccess)

                seriloggers.Add(seriloggerResult.Value);
        });

        // Assert
        var firstLogger = seriloggers.First();

        Assert.All(seriloggers, logger => Assert.Same(firstLogger, logger));
    }

    private static SerilogInstance CreateInvalidSerilogInstance()
    {
        var configuration = new Mock<IConfiguration>();

        var serilogInstance = new SerilogInstance(configuration.Object);

        return serilogInstance;
    }

    [Fact]
    public void GetInstance_MissingConfiguration_ShouldFail()
    {
        // Arrange
        var serilogInstance = CreateInvalidSerilogInstance();

        // Act
        (_, bool seriloggerFailure, _, var seriloggerError) = serilogInstance.Get();

        // Assert
        Assert.True(seriloggerFailure);

        Assert.NotNull(seriloggerError.Exception);
    }
}