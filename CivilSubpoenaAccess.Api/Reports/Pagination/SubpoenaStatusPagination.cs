using CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;

namespace CivilSubpoenaAccess.Api.Reports.Pagination;


public class SubpoenaStatusPagination : ObjectType<TransactionsBySubpoenaStatus>
{
    protected override void Configure
    (
        IObjectTypeDescriptor<TransactionsBySubpoenaStatus> typeDescriptor
    )
    {
        typeDescriptor.Field(x => x.Approved)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.Rejected)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.PendingApproval)
            .UsePaging()
            .UseFiltering()
            .UseSorting();

        typeDescriptor.Field(x => x.PendingPayment)
            .UsePaging()
            .UseFiltering()
            .UseSorting();
    }
}