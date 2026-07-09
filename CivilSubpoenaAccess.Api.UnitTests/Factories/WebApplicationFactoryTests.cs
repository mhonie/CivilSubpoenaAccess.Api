using CSharpFunctionalExtensions;
using Moq;

using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Bootstrap;
using CivilSubpoenaAccess.Api.Factories;
using CivilSubpoenaAccess.Api.WebHosting;
using Microsoft.Extensions.Configuration;

namespace CivilSubpoenaAccess.Api.UnitTests.Factories;


public class WebApplicationFactoryTests
{
    private static WebHostSettings WebHostSettings()
    {
        return new WebHostSettings
        {
            ApiEndpoints = new ApiEndpoints { BasePath = "/civil-subpoena-access-api" },

            CorsSettings = new CorsSettings { AllowedOrigins = [] },

            KestrelSettings = new KestrelSettings
            {
                HttpPort = 1234, 

                HttpsPort = 1235
            }
        };
    }

    private static StartupErrorTracker StartupErrorTracker()
    {
        return new StartupErrorTracker();
    }

    [Fact]
    public void Create_ValidServiceProviders_ShouldSucceed()
    {
        // Arrange
        const string environmentNameKey = "CIVIL_SUBPOENA_ACCESS_API_ENVIRONMENT_NAME";

        var startupServiceCallbacks = new StartupServiceCallbacks
        {
            EnvironmentNameKey = environmentNameKey
        };

        var entityFrameworkCallbacks = new EntityFrameworkCallbacks();

        var configuration = new ConfigurationBuilder()
            
            .AddInMemoryCollection([

                KeyValuePair.Create<string, string?>
                (
                    "WebHostSettings:ApiEndpoints:BasePath",

                    "/civil-subpoena-access-api"
                ),

                KeyValuePair.Create<string, string?>
                (
                    "WebHostSettings:CorsSettings:AllowedOrigins:5200",

                    "http://localhost:5173"
                ),

                KeyValuePair.Create<string, string?>
                (
                    "WebHostSettings:KestrelSettings:HttpPort",

                    "5000"
                ),

                KeyValuePair.Create<string, string?>
                (
                    "WebHostSettings:KestrelSettings:HttpsPort",

                    "5001"
                ),
             ])

            .Build();

        var webAppFactory = new WebApplicationFactory
        (
            configuration,

            StartupErrorTracker()
        ) 
        {
           StartupServiceCallbacks  = startupServiceCallbacks,

           EntityFrameworkCallbacks = entityFrameworkCallbacks
        };

        // Act
        (_, bool webApplicationFailure, var webApplication, var startupError) =

            webAppFactory.Create([]);

        // Assert
        Assert.False(webApplicationFailure);

        Assert.NotNull(webApplication);

        Assert.Null(startupError);
    }

    [Fact]
    public void Create_InvalidStartupServiceFactories_ShouldFail()
    {
        var startupServiceCallbacks = new Mock<IStartupServiceCallbacks>();

        startupServiceCallbacks.Setup
        (
            x => x.ConfigFile(It.IsAny<StartupErrorTracker>())
        )
            .Throws(new Exception("startup failure"));

        var entityFrameworkCallbacks = new EntityFrameworkCallbacks();

        var webAppFactory = new WebApplicationFactory
        (
            new Mock<IConfiguration>().Object,

            StartupErrorTracker()
        )
        {
            StartupServiceCallbacks = startupServiceCallbacks.Object,

            EntityFrameworkCallbacks = entityFrameworkCallbacks
        };

        // Act
        (_, bool webApplicationFailure, var webApplication, var startupError) =

            webAppFactory.Create([]);

        // Assert
        Assert.True(webApplicationFailure);

        Assert.Null(webApplication);

        Assert.NotNull(startupError); 
        
        Assert.True
        (
            startupError.InjectionError is not null ||

            startupError.InitializationError is not null
        );
    }
}