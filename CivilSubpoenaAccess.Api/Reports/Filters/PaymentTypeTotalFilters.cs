namespace CivilSubpoenaAccess.Api.Reports.Filters;


public class PaymentTypeTotalFilters
{
    public required CartFilter AmericanExpress { get; init; }

    public required CartFilter DiscoverCard { get; init; }

    public required CartFilter Mastercard { get; init; }

    public required CartFilter VisaCard { get; init; }

    public required CartFilter CitySolicitor{ get; init; }

    public required CartFilter CityLaw { get; init; }

    public CartFilter? InFormaPauperis { get; init; }

    public required CaseFilter WalkIn { get; init; }
}