namespace CivilSubpoenaAccess.Api.Database.Entities;


public class Cart
{
    public required string TransactionId { get; init; }

    public string? PurchaseType { get; init; }

    public string? UserName { get; init; }

    public string? ConfirmationNumber { get; init; }

    public string? Email { get; init; }

    public string? AdditionalEmail1 { get; init; }

    public string? AdditionalEmail2 { get; init; }

    public string? CreditCardType { get; init; }

    public string? PaymentNetworkReference { get; init; }

    public int? NumberOfItems { get; init; }

    public int? TotalPages { get; init; }

    public decimal? TotalPrice { get; init; }

    public required DateTime InitiatedDate { get; init; }

    public required string Status { get; init; }

    public DateTime? PaidDate { get; init; }
}