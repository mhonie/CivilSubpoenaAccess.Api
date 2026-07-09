namespace CivilSubpoenaAccess.Api.ErrorTypes;


public class InitializationError // error when we instantiate and object
{
    public ConfigError? ConfigError { get; init; }

    public EnvFileError? EnvFileError { get; init; }

    public LoggerError? LoggerError { get; init; }
}

public class StartupError // error when the application starts
{
    public InitializationError? InitializationError { get; init; }

    public InjectionError? InjectionError { get; init; }
}

public class StartupErrorTracker
{
    public StartupError? StartupError { get; set; }
}