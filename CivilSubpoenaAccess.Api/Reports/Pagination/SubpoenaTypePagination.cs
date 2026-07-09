using CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;

namespace CivilSubpoenaAccess.Api.Reports.Pagination;


public class SubpoenaTypePagination : ObjectType<TransactionsBySubpoenaType>
{
    protected override void Configure
    (
        IObjectTypeDescriptor<TransactionsBySubpoenaType> typeDescriptor
    )
    {
        typeDescriptor.Field(x => x.Attend)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.Produce)
            .UsePaging()
            .UseFiltering()
            .UseSorting();
    }
}