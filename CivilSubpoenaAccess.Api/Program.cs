using CSharpFunctionalExtensions;

using CivilSubpoenaAccess.Api.Bootstrap;
using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Factories;


const string EnvironmentNameKey = "CIVIL_SUBPOENA_ACCESS_API_ENVIRONMENT_NAME";

static void Run(string[] commandLineArgs)
{

    var startupServiceCallbacks = new StartupServiceCallbacks
    {
        EnvironmentNameKey = EnvironmentNameKey
    };

    var webHostingErrorTracker = new StartupErrorTracker();

    var webHostSettingsFactory = new ConfigurationFactory
    (
        webHostingErrorTracker
    )
    {
        StartupServiceCallbacks = startupServiceCallbacks
    };

    (_, bool configFailure, var appConfiguration, _) = 
        
        webHostSettingsFactory.Create();

    if (configFailure) return;

    var entityFrameworkCallbacks = new EntityFrameworkCallbacks();

    var webAppErrorTracker = new StartupErrorTracker();

    var webAppFactory = new WebApplicationFactory
    (
        appConfiguration, 

        webAppErrorTracker
    )
    {
        StartupServiceCallbacks = startupServiceCallbacks,

        EntityFrameworkCallbacks = entityFrameworkCallbacks
    };

    (_, bool webAppFailure, var webApplication, _) = 
        
        webAppFactory.Create(commandLineArgs);

    if (webAppFailure) return;

    webApplication.Run();
}

Run(args);