namespace CivilSubpoenaAccess.Api.Reports.Filters;


public class PaymentTypeFeeFilters
{
    public required CartFilter AmericanExpress { get; init; }

    public required CartFilter DiscoverCard { get; init; }

    public required CartFilter Mastercard { get; init; }

    public required CartFilter VisaCard { get; init; }
}