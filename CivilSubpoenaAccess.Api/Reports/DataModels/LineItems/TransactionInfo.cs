namespace CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;


public class TransactionInfo
{
    public required string TransactionId { get; init; }

    public string? Username { get; init; }

    public DateTime? FilingDate { get; init; }

    public DateTime? ApprovedDate { get; init; }

    public DateTime? RejectedDate { get; init; }

    public DateTime? PaidDate { get; init; }

    public required int ItemCount { get; init; }

    public required decimal PaymentAmount { get; init; }
}