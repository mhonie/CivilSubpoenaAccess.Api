namespace CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;


public class TransactionsByUserType
{
    public IQueryable<TransactionInfo>? Public { get; init; }

    public IQueryable<TransactionInfo>? EFilingAttorney { get; init; }

    public IQueryable<TransactionInfo>? EFilingProSe { get; init; }

    public IQueryable<TransactionInfo>? CityLaw { get; init; }

    public IQueryable<TransactionInfo>? CitySolicitor { get; init; }

    public IQueryable<TransactionInfo>? FilingService { get; init; }

    public string? Error { get; init; }
}