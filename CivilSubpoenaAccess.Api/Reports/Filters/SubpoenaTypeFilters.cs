namespace CivilSubpoenaAccess.Api.Reports.Filters;


public class SubpoenaTypeFilters
{
    public required CaseFilter Attend { get; init; }

    public required CaseFilter Produce { get; init; }
}