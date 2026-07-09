using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using CivilSubpoenaAccess.Api.Database;
using CivilSubpoenaAccess.Api.ErrorTypes;

namespace CivilSubpoenaAccess.Api.Bootstrap;


public interface IEntityFrameworkCallbacks
{
    Action<IServiceProvider, DbContextOptionsBuilder> DbContextOptions
    (
        StartupErrorTracker startupErrorTracker
    );
}

public class EntityFrameworkCallbacks : IEntityFrameworkCallbacks
{
    public Action<IServiceProvider, DbContextOptionsBuilder> DbContextOptions
    (
        StartupErrorTracker startupErrorTracker
    )
    {
        return (serviceProvider, dbContextOptionsBuilder) =>
        {
            var connectionStringBuilder =

                serviceProvider.GetRequiredService<IConnectionStringBuilder>();

            var connectionStringOption =

                serviceProvider.GetRequiredService<IOptions<EfilingConnectionString>>();

            var efilingConnectionString = connectionStringOption.Value;

            (_, bool envFileFailure, string connectionString, var envFileError) =

                connectionStringBuilder.Build(efilingConnectionString.Format);

            if (envFileFailure)
            {
                var initError = new InitializationError { EnvFileError = envFileError };

                var startupError = new StartupError { InitializationError = initError };

                startupErrorTracker.StartupError = startupError;

                throw envFileError.Exception ?? new Exception(envFileError.Message);
            }

            dbContextOptionsBuilder.UseOracle(connectionString);
        };
    }
}