namespace CivilSubpoenaAccess.Api.Reports.DataModels.Aggregates;


public class UserTypeCounts
{
    public int Public { get; init; }

    public int EFilingAttorney { get; init; }

    public int EFilingProSe { get; init; }

    public int CityLaw { get; init; }

    public int CitySolicitor { get; init; }

    public int FilingService { get; init; }

    public string? Error { get; init; }
}