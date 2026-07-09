namespace CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;


public class TransactionsByPaymentType
{
    public IQueryable<TransactionInfo>? InFormaPauperis { get; init; }

    public IQueryable<TransactionInfo>? WalkIn { get; init; }

    public IQueryable<TransactionInfo>? CityLaw { get; init; }

    public IQueryable<TransactionInfo>? CitySolicitor { get; init; }

    public IQueryable<TransactionInfo>? AmericanExpress { get; init; }

    public IQueryable<TransactionInfo>? DiscoverCard { get; init; }

    public IQueryable<TransactionInfo>? Mastercard { get; init; }

    public IQueryable<TransactionInfo>? VisaCard { get; init; }

    public string? Error { get; init; }
}