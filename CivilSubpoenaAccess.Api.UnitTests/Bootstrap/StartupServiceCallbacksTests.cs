using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

using CivilSubpoenaAccess.Api.Configuration;
using CivilSubpoenaAccess.Api.Environment;
using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Bootstrap;
using CivilSubpoenaAccess.Api.Logging;

namespace CivilSubpoenaAccess.Api.UnitTests.Bootstrap;


public class StartupServiceCallbacksTests
{
    private const string EnvironmentNameKey = "CIVIL_SUBPOENA_ACCESS_API_ENVIRONMENT_NAME";

    private static ServiceProvider ServiceProvider
    (
        params (Type serviceType, object implementation)[] services
    )
    {
        var serviceCollection = new ServiceCollection();

        foreach ((Type serviceType, object implementation) in services)
        {
            serviceCollection.AddSingleton(serviceType, implementation);
        }

        return serviceCollection.BuildServiceProvider();
    }

    [Fact]
    public void ConfigFile_ValidEnvironmentName_ShouldSucceed()
    {
        // Arrange
        var envLoader = new Mock<IEnvLoader>();

        envLoader.Setup(x => x.Load())
            
            .Returns(UnitResult.Success<EnvFileError>());

        var envReader = new Mock<IEnvReader>();

        envReader.Setup(x => x.ReadValue(EnvironmentNameKey))

            .Returns(Result.Success<string, EnvFileError>("Development"));

        var startupServiceCallbacks = new StartupServiceCallbacks
        {
            EnvironmentNameKey = EnvironmentNameKey,
        };

        var startupErrorTracker = new StartupErrorTracker();

        var serviceProvider = ServiceProvider
        (
            (typeof(IEnvLoader), envLoader.Object),

            (typeof(IEnvReader), envReader.Object)
        );

        // Act
        var configFileFactory = startupServiceCallbacks.ConfigFile(startupErrorTracker);

        var configFile = configFileFactory(serviceProvider);

        // Assert
        Assert.NotNull(configFile);

        Assert.Null(startupErrorTracker.StartupError?.InitializationError);
    }

    [Fact]
    public void ConfigFile_InvalidEnvironmentName_ShouldFail()
    {
        // Arrange
        var envError = new EnvFileError { Message = "Failed to read environment variable" };

        var envLoader = new Mock<IEnvLoader>();

        envLoader.Setup(x => x.Load())
            
            .Returns(UnitResult.Success<EnvFileError>());

        var envReader = new Mock<IEnvReader>();

        envReader.Setup(x => x.ReadValue(EnvironmentNameKey))

            .Returns(Result.Failure<string, EnvFileError>(envError));

        var startupServiceCallbacks = new StartupServiceCallbacks
        {
            EnvironmentNameKey = EnvironmentNameKey,
        };

        var startupErrorTracker = new StartupErrorTracker();

        var serviceProvider = ServiceProvider
        (
            (typeof(IEnvLoader), envLoader.Object),

            (typeof(IEnvReader), envReader.Object)
        );

        // Act
        var configFileFactory = startupServiceCallbacks.ConfigFile(startupErrorTracker);

        Exception? exception = Record.Exception(() => configFileFactory(serviceProvider));

        // Assert
        Assert.NotNull(exception);

        Assert.NotNull(startupErrorTracker.StartupError?.InitializationError);

        Assert.NotNull(startupErrorTracker.StartupError.InitializationError!.EnvFileError);
    }

    [Fact]
    public void Configuration_ValidConfig_ShouldSucceed()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>().Object;

        var configLoader = new Mock<IConfigLoader>();

        configLoader.Setup(x => x.Load())

            .Returns(Result.Success<IConfiguration, ConfigError>(configuration));

        var startupServiceCallbacks = new StartupServiceCallbacks
        {
            EnvironmentNameKey = EnvironmentNameKey,
        };

        var startupErrorTracker = new StartupErrorTracker();

        var serviceProvider = ServiceProvider
        (
            (typeof(IConfigLoader), configLoader.Object)
        );

        // Act
        var configFactory = startupServiceCallbacks.Configuration(startupErrorTracker);

        var newConfiguration = configFactory(serviceProvider);

        // Assert
        Assert.NotNull(newConfiguration);

        Assert.Null(startupErrorTracker.StartupError?.InitializationError);
    }

    [Fact]
    public void Configuration_InvalidConfig_ShouldFail()
    {
        // Arrange
        var configError = new ConfigError { Message = "Failed to load config file" };

        var configLoader = new Mock<IConfigLoader>();

        configLoader.Setup(x => x.Load())

            .Returns(Result.Failure<IConfiguration, ConfigError>(configError));

        var startupServiceCallbacks = new StartupServiceCallbacks
        {
            EnvironmentNameKey = EnvironmentNameKey,
        };

        var startupErrorTracker = new StartupErrorTracker();

        var serviceProvider = ServiceProvider
        (
            (typeof(IConfigLoader), configLoader.Object)
        );

        // Act
        var configFactory = startupServiceCallbacks.Configuration(startupErrorTracker);

        Exception? exception = Record.Exception(() => configFactory(serviceProvider));

        // Assert
        Assert.NotNull(exception);

        var initError = startupErrorTracker.StartupError?.InitializationError;

        Assert.NotNull(initError);

        Assert.NotNull(initError.ConfigError);
    }

    [Fact]
    public void Serilogger_ValidLogger_ShouldSucceed()
    {
        // Arrange
        var serilogger = new Mock<ILogger>().Object;

        var serilogInstance = new Mock<ISerilogInstance>();

        serilogInstance.Setup(x => x.Get())

            .Returns(Result.Success<ILogger, LoggerError>(serilogger));

        var startupServiceCallbacks = new StartupServiceCallbacks
        {
            EnvironmentNameKey = EnvironmentNameKey,
        };

        var startupErrorTracker = new StartupErrorTracker();

        var serviceProvider = ServiceProvider
        (
            (typeof(ISerilogInstance), serilogInstance.Object)
        );

        // Act
        var loggerFactory = startupServiceCallbacks.Serilogger(startupErrorTracker);

        var createdLogger = loggerFactory(serviceProvider);

        // Assert
        Assert.NotNull(createdLogger);

        Assert.Null(startupErrorTracker.StartupError?.InitializationError);
    }

    [Fact]
    public void Serilogger_InvalidLogger_ShouldFail()
    {
        // Arrange
        var loggerError = new LoggerError { Message = "logger failure" };

        var serilogInstance = new Mock<ISerilogInstance>();

        serilogInstance.Setup(x => x.Get())

            .Returns(Result.Failure<ILogger, LoggerError>(loggerError));

        var startupServiceCallbacks = new StartupServiceCallbacks
        {
            EnvironmentNameKey = EnvironmentNameKey,
        };

        var startupErrorTracker = new StartupErrorTracker();

        var serviceProvider = ServiceProvider
        (
            (typeof(ISerilogInstance), serilogInstance.Object)
        );

        // Act
        var loggerFactory = startupServiceCallbacks.Serilogger(startupErrorTracker);

        Exception? exception = Record.Exception(() => loggerFactory(serviceProvider));

        // Assert
        Assert.NotNull(exception);

        var initError = startupErrorTracker.StartupError?.InitializationError;

        Assert.NotNull(initError);

        Assert.NotNull(initError.LoggerError);
    }
}