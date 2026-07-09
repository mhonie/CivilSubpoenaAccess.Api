namespace CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;


public class TransactionsBySubpoenaType
{
    public IQueryable<TransactionInfo>? Attend { get; init; }

    public IQueryable<TransactionInfo>? Produce { get; init; }

    public string? Error { get; init; }
}