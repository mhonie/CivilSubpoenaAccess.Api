namespace CivilSubpoenaAccess.Api.Reports.DataModels.Aggregates;


public class SubpoenaTypeCounts
{
    public int Attend { get; init; }

    public int Produce { get; init; }

    public string? Error { get; init; }
}