namespace CivilSubpoenaAccess.Api.Reports.Filters;


public class PaymentTypeCountFilters
{
    public required CaseCartFilter AmericanExpress { get; init; }

    public required CaseCartFilter DiscoverCard { get; init; }

    public required CaseCartFilter Mastercard { get; init; }

    public required CaseCartFilter VisaCard { get; init; }

    public required CaseCartFilter CitySolicitor { get; init; }

    public required CaseCartFilter CityLaw { get; init; }

    public required CaseCartFilter InFormaPauperis { get; init; }

    public required CaseCartFilter WalkIn { get; init; }
}