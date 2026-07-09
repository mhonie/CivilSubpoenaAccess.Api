namespace CivilSubpoenaAccess.Api.Database.Schema;


public class FieldInfo
{
    public required string Name { get; init; }

    public int? MaxLength { get; init; }

    public string? ColumnType { get; init; }

    public int? Precision { get; init; }

    public int? Scale { get; init; }

    public bool Nullable { get; init; }
}