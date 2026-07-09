namespace CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;


public class TransactionsBySubpoenaStatus
{
    public IQueryable<TransactionInfo>? Approved { get; init; }

    public IQueryable<TransactionInfo>? Rejected { get; init; }

    public IQueryable<TransactionInfo>? PendingPayment { get; init; }

    public IQueryable<TransactionInfo>? PendingApproval { get; init; }

    public string? Error { get; init; }
}