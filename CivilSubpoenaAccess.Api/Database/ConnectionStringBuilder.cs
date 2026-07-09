using CSharpFunctionalExtensions;

using CivilSubpoenaAccess.Api.Environment;
using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Serialization;

namespace CivilSubpoenaAccess.Api.Database;


public interface IConnectionStringBuilder
{
    Result<string, EnvFileError> Build(string formatString);
}

public class ConnectionStringBuilder(IEnvReader envReader, ILogger serilogger) 

    : IndentingFormatter, IConnectionStringBuilder
{
    private const string UserName = "EFILING_DATABASE_USER_NAME";

    private const string Password = "EFILING_DATABASE_PASSWORD";

    private const string ServerName = "EFILING_DATABASE_SERVER_NAME";

	private const string DatabasePort = "EFILING_DATABASE_PORT";

    private const string DatabaseName = "EFILING_DATABASE_NAME";

	private void BuildingConnectionString()
    {
        serilogger.Information("Building connection string...\n");
    }

    private void ErrorBuildingConnectionString(EnvFileError envFileError)
    {
        string envFileErrorJson = ToJson(envFileError);

        serilogger.Error
        (
            envFileError.Exception,

            "Error building connection string\n\n{envFileError}\n",

            envFileErrorJson
        );
    }

	public Result<string, EnvFileError> Build(string formatString)
    {
        try
        {
            BuildingConnectionString();

            (bool _, bool userIdFailure, string? userId, var databaseUserIdError)

                = envReader.ReadValue(UserName);

            if (userIdFailure)
            {
                ErrorBuildingConnectionString(databaseUserIdError);

                return databaseUserIdError;
            }

            (bool _, bool passwordFailure, string? password, var databasePasswordError)

                = envReader.ReadValue(Password);

            if (passwordFailure)
            {
                ErrorBuildingConnectionString(databasePasswordError);

                return databasePasswordError;
            }

            (bool _, bool serverNameFailure, string? serverName, var serverNameError)

                = envReader.ReadValue(ServerName);

            if (serverNameFailure)
            {
                ErrorBuildingConnectionString(serverNameError);

                return serverNameError;
            }

            (bool _, bool databasePortFailure, string? databasePort, var databasePortError)

                = envReader.ReadValue(DatabasePort);

            if (databasePortFailure)
            {
                ErrorBuildingConnectionString(databasePortError);

                return databasePortError;
            }

            (bool _, bool databaseNameFailure, string? databaseName, var databaseNameError)

                = envReader.ReadValue(DatabaseName);

            if (databaseNameFailure)
            {
                ErrorBuildingConnectionString(databaseNameError);

                return databaseNameError;
            }

            string connectionString =

                string.Format(formatString, userId, password, serverName, databasePort, databaseName);

            return connectionString;
        }
        catch (Exception connectionStringException)
        {
            var connectionStringError = new EnvFileError
            {
                Message = "Error building Oracle connection string",

                Exception = connectionStringException
            };

            ErrorBuildingConnectionString(connectionStringError);

            return connectionStringError;
        }
    }
}