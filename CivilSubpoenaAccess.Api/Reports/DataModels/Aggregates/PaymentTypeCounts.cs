namespace CivilSubpoenaAccess.Api.Reports.DataModels.Aggregates;


public class PaymentTypeCounts
{
    public int InFormaPauperis { get; init; }

    public int CitySolicitor { get; init; }

    public int CityLaw { get; init; }

    public int WalkIn { get; init; }

    public int AmericanExpress { get; init; }

    public int DiscoverCard { get; init; }

    public int Mastercard { get; init; }

    public int VisaCard { get; init; }

    public string? Error { get; init; }
}

public class PaymentTypeCountsByPeriod
{
    public required DateTime StartDate { get; init; }

    public required DateTime EndDate { get; init; }

    public required PaymentTypeCounts PaymentTypeCount { get; init; }
}

public class PaymentTypeCountsByMonth
{
    public PaymentTypeCountsByPeriod[]? PaymentTypeCountsByPeriod { get; init; }

    public string? Error { get; init; }
}