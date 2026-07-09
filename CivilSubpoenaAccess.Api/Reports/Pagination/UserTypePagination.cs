using CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;

namespace CivilSubpoenaAccess.Api.Reports.Pagination;


public class UserTypePagination : ObjectType<TransactionsByUserType>
{
    protected override void Configure
    (
        IObjectTypeDescriptor<TransactionsByUserType> typeDescriptor
    )
    {
        typeDescriptor.Field(x => x.Public)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.EFilingAttorney)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.EFilingProSe)
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

        typeDescriptor.Field(x => x.FilingService)
            .UsePaging()
            .UseFiltering()
            .UseSorting();
    }
}