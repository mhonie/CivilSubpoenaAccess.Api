namespace CivilSubpoenaAccess.Api.Configuration;


/// <summary>
/// File system information about the <c>appsettings.[Environment].json</c> files
/// </summary>
public interface IConfigFile
{
    string Directory { get; }

    string DefaultFileName { get; }

    string FileName { get; }
}

/// <inheritdoc cref="IConfigFile"/>
public class ConfigFile(string environmentName) : IConfigFile
{
    private const string fileNamePrefix = "appsettings";

    private const string fileExtension = "json";

    public string Directory => AppDomain.CurrentDomain.BaseDirectory;

    public string DefaultFileName => $"{fileNamePrefix}.{fileExtension}";

    public string FileName => $"{fileNamePrefix}.{environmentName}.{fileExtension}";
}