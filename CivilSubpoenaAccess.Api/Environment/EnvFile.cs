using CSharpFunctionalExtensions;

using CivilSubpoenaAccess.Api.ErrorTypes;

using Path = System.IO.Path;

namespace CivilSubpoenaAccess.Api.Environment;


/// <summary>
/// File system information about the .env file.
/// </summary>
public interface IEnvFile
{
    Result<string, EnvFileError> FilePath();
}


/// <inheritdoc cref="IEnvFile"/>
public class EnvFile : IEnvFile
{
    private const string envFileName = ".env";

    public Result<string, EnvFileError> FilePath()
    {
        try
        {
            string envDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string envFilePath = Path.Combine(envDirectory, envFileName);

            return envFilePath;
        }
        catch (Exception envFilePathException)
        {
            var envFilePathError = new EnvFileError
            {
                Message = $"Error determining file path for {envFileName}",

                Exception = envFilePathException
            };

            return envFilePathError;
        }
    }
}