namespace CivilSubpoenaAccess.Api.Reports.Filters;


public class SubpoenaStatusFilters
{
    public required CaseFilter Approved { get; init; }

    public required CaseFilter Rejected { get; init; }

    public required CaseFilter PendingPayment { get; init; }

    public required CaseFilter PendingApproval { get; init; }
}