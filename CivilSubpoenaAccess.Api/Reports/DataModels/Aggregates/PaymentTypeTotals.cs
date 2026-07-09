namespace CivilSubpoenaAccess.Api.Reports.DataModels.Aggregates;


public class PaymentTypeTotals
{
    public decimal? InFormaPauperis { get; init; }

    public decimal? CityLaw { get; init; }

    public decimal? CitySolicitor { get; init; }

    public decimal? WalkIn { get; init; }

    public decimal? AmericanExpress { get; init; }
    
    public decimal? DiscoverCard { get; init; }

    public decimal? Mastercard { get; init; }
    
    public decimal? VisaCard { get; init; }

    public string? Error { get; init; }
}