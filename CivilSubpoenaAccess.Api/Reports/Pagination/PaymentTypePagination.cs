using CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;

namespace CivilSubpoenaAccess.Api.Reports.Pagination;


public class PaymentTypePagination : ObjectType<TransactionsByPaymentType>
{
    protected override void Configure
    (
        IObjectTypeDescriptor<TransactionsByPaymentType> typeDescriptor
    )
    {
        typeDescriptor.Field(x => x.InFormaPauperis)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.WalkIn)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.CityLaw)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.CitySolicitor)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.AmericanExpress)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.DiscoverCard)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.Mastercard)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.VisaCard)
            .UsePaging()
            .UseFiltering()
            .UseSorting();
    }
}