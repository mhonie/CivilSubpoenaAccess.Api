using System.ComponentModel.DataAnnotations;

namespace CivilSubpoenaAccess.Api.WebHosting;


public class ApiEndpoints
{
    [Required]
    public required string BasePath { get; init; }
}

public class CorsSettings
{
    [Required]
    public required string[] AllowedOrigins { get; init; }
}

public class KestrelSettings
{
    [Required]
    public required int HttpPort { get; init; }

    [Required]
    public required int HttpsPort { get; init; }
}

public class WebHostSettings
{
    public required CorsSettings CorsSettings { get; init; }

    public required ApiEndpoints ApiEndpoints { get; init; }

    public KestrelSettings? KestrelSettings { get; init; }
}