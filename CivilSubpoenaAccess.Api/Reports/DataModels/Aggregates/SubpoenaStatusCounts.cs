namespace CivilSubpoenaAccess.Api.Reports.DataModels.Aggregates;


public class SubpoenaStatusCounts
{
    public int Approved { get; init; }

    public int Rejected { get; init; }

    public int PendingPayment { get; init; }

    public int PendingApproval { get; init; }

    public string? Error { get; init; }
}