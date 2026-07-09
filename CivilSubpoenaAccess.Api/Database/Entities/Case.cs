namespace CivilSubpoenaAccess.Api.Database.Entities;


public class Case
{
    public required string TransactionId { get; init; }

    public required int SubpoenaNumber { get; init; }

    public string? CaseId { get; init; }

    public string? SubpoenaType { get; init; }

    public string? Addressee { get; init; }

    public string? Address1 { get; init; }

    public string? Address2 { get; init; }

    public string? Address3 { get; init; }

    public string? City { get; init; }

    public string? State { get; init; }

    public string? ZipCode { get; init; }

    public DateTime? EventDate { get; init; }

    public string? Location { get; init; }

    public DateTime? DateFiled { get; init; }

    public string? EventTime { get; init; }

    public string? Testify { get; init; }

    public string? Evidence { get; init; }

    public string? Attorney { get; init; }

    public string? AttorneyAddress1 { get; init; }

    public string? AttorneyAddress2 { get; init; }

    public string? AttorneyAddress3 { get; init; }

    public string? AttorneyTelephone { get; init; }

    public string? AttorneyId { get; init; }

    public string? AttorneySubpoenaId { get; init; }

    public string? AttorneyFor { get; init; }

    public string? Email { get; init; }

    public string? ConfirmationNumber { get; init; }

    public string? UserName { get; init; }

    public string? PaymentIndicator { get; init; }

    public string? Status { get; init; }

    public string? ApprovalIndicator { get; init; }

    public string? ApprovingClerk { get; init; }

    public DateTime? ApproveDate { get; init; }

    public DateTime? FeePaidDate { get; init; }

    public string? FeePaymentType { get; init; }

    public decimal? FeePaidAmount { get; init; }

    public string? Narrative { get; init; }

    public string? UserId { get; init; }

    public string? UserReferenceNumber { get; init; }

    public string? EFilingIndicator { get; init; }
}