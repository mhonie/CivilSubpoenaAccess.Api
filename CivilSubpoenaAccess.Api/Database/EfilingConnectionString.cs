using System.ComponentModel.DataAnnotations;

namespace CivilSubpoenaAccess.Api.Database;

public class EfilingConnectionString
{
    [Required]
    public required string Format { get; init; }
}