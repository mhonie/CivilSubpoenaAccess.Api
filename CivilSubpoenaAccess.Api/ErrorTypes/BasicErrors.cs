using Destructurama.Attributed;
using Newtonsoft.Json;
using CivilSubpoenaAccess.Api.Database.Entities;

namespace CivilSubpoenaAccess.Api.ErrorTypes;


public abstract class GenericError
{
    public required string Message { get; init; }

    [JsonIgnore]
    [NotLogged]
    public Exception? Exception { get; init; }
}

public class InjectionError : GenericError;

public class LoggerError : GenericError;

public class EnvFileError : GenericError
{
    public string? FilePath { get; init; }

    public string? VariableName { get; init; }
}

public class ConfigError : GenericError
{
    public string? FileName { get; init; }
}

public class DatabaseError : GenericError
{
    public Case? Case { get; init; }

    public Case[]? Cases { get; init; }

    public Cart? Cart { get; init; }

    public Cart[]? Carts { get; init; }

    public User? User { get; init; }

    public User[]? Users { get; init; }
}

public class NotFoundError : DatabaseError;
