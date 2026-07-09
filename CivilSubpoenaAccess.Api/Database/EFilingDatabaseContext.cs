using Microsoft.EntityFrameworkCore;

using CivilSubpoenaAccess.Api.Database.Schema.Tables;
using CivilSubpoenaAccess.Api.Database.Entities;

namespace CivilSubpoenaAccess.Api.Database;


public class EFilingDatabaseContext
(
    ICaseTable caseTable,

    ICartTable cartTable,

    IUserTable userTable,

    DbContextOptions<EFilingDatabaseContext> options
)
    : DbContext(options)
{
    public virtual DbSet<Case> Cases { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    private void BuildCases(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Case>(entity =>
        {
            entity.ToTable(caseTable.Name);

            entity.HasKey(x => new { x.TransactionId, x.SubpoenaNumber });

            entity.HasIndex(x => new { x.TransactionId, x.SubpoenaNumber })
                .HasDatabaseName(caseTable.UniqueIndex)
                .IsUnique();

            entity.Property(x => x.TransactionId)
                .HasColumnName(caseTable.TransactionIdField.Name)
                .HasMaxLength(caseTable.TransactionIdField.MaxLength!.Value);

            entity.Property(x => x.SubpoenaNumber)
                .HasColumnName(caseTable.SubpoenaNumberField.Name)
                .HasPrecision
                (
                    caseTable.SubpoenaNumberField.Precision!.Value,
                    caseTable.SubpoenaNumberField.Scale!.Value
                );

            entity.Property(x => x.CaseId)
                .HasColumnName(caseTable.CaseIdField.Name)
                .HasMaxLength(caseTable.CaseIdField.MaxLength!.Value);

            entity.Property(x => x.SubpoenaType)
                .HasColumnName(caseTable.SubpoenaTypeField.Name)
                .HasMaxLength(caseTable.SubpoenaTypeField.MaxLength!.Value);

            entity.Property(x => x.Addressee)
                .HasColumnName(caseTable.AddresseeField.Name)
                .HasMaxLength(caseTable.AddresseeField.MaxLength!.Value);

            entity.Property(x => x.Address1)
                .HasColumnName(caseTable.Address1Field.Name)
                .HasMaxLength(caseTable.Address1Field.MaxLength!.Value);

            entity.Property(x => x.Address2)
                .HasColumnName(caseTable.Address2Field.Name)
                .HasMaxLength(caseTable.Address2Field.MaxLength!.Value);

            entity.Property(x => x.Address3)
                .HasColumnName(caseTable.Address3Field.Name)
                .HasMaxLength(caseTable.Address3Field.MaxLength!.Value);

            entity.Property(x => x.City)
                .HasColumnName(caseTable.CityField.Name)
                .HasMaxLength(caseTable.CityField.MaxLength!.Value);

            entity.Property(x => x.State)
                .HasColumnName(caseTable.StateField.Name)
                .HasMaxLength(caseTable.StateField.MaxLength!.Value);

            entity.Property(x => x.ZipCode)
                .HasColumnName(caseTable.ZipCodeField.Name)
                .HasMaxLength(caseTable.ZipCodeField.MaxLength!.Value);

            entity.Property(x => x.EventDate)
                .HasColumnName(caseTable.EventDateField.Name)
                .HasColumnType(caseTable.EventDateField.ColumnType);

            entity.Property(x => x.Location)
                .HasColumnName(caseTable.LocationField.Name)
                .HasMaxLength(caseTable.LocationField.MaxLength!.Value);

            entity.Property(x => x.DateFiled)
                .HasColumnName(caseTable.DateFiledField.Name)
                .HasColumnType(caseTable.DateFiledField.ColumnType);

            entity.Property(x => x.EventTime)
                .HasColumnName(caseTable.EventTimeField.Name)
                .HasMaxLength(caseTable.EventTimeField.MaxLength!.Value);

            entity.Property(x => x.Testify)
                .HasColumnName(caseTable.TestifyField.Name)
                .HasMaxLength(caseTable.TestifyField.MaxLength!.Value);

            entity.Property(x => x.Evidence)
                .HasColumnName(caseTable.EvidenceField.Name)
                .HasMaxLength(caseTable.EvidenceField.MaxLength!.Value);

            entity.Property(x => x.Attorney)
                .HasColumnName(caseTable.AttorneyField.Name)
                .HasMaxLength(caseTable.AttorneyField.MaxLength!.Value);

            entity.Property(x => x.AttorneyAddress1)
                .HasColumnName(caseTable.AttorneyAddress1Field.Name)
                .HasMaxLength(caseTable.AttorneyAddress1Field.MaxLength!.Value);

            entity.Property(x => x.AttorneyAddress2)
                .HasColumnName(caseTable.AttorneyAddress2Field.Name)
                .HasMaxLength(caseTable.AttorneyAddress2Field.MaxLength!.Value);

            entity.Property(x => x.AttorneyAddress3)
                .HasColumnName(caseTable.AttorneyAddress3Field.Name)
                .HasMaxLength(caseTable.AttorneyAddress3Field.MaxLength!.Value);

            entity.Property(x => x.AttorneyTelephone)
                .HasColumnName(caseTable.AttorneyTelephoneField.Name)
                .HasMaxLength(caseTable.AttorneyTelephoneField.MaxLength!.Value);

            entity.Property(x => x.AttorneyId)
                .HasColumnName(caseTable.AttorneyIdField.Name)
                .HasMaxLength(caseTable.AttorneyIdField.MaxLength!.Value);

            entity.Property(x => x.AttorneySubpoenaId)
                .HasColumnName(caseTable.AttorneySubpoenaIdField.Name)
                .HasMaxLength(caseTable.AttorneySubpoenaIdField.MaxLength!.Value);

            entity.Property(x => x.AttorneyFor)
                .HasColumnName(caseTable.AttorneyForField.Name)
                .HasMaxLength(caseTable.AttorneyForField.MaxLength!.Value);

            entity.Property(x => x.Email)
                .HasColumnName(caseTable.EmailField.Name)
                .HasMaxLength(caseTable.EmailField.MaxLength!.Value);

            entity.Property(x => x.ConfirmationNumber)
                .HasColumnName(caseTable.ConfirmationNumberField.Name)
                .HasMaxLength(caseTable.ConfirmationNumberField.MaxLength!.Value);

            entity.Property(x => x.UserName)
                .HasColumnName(caseTable.UserNameField.Name)
                .HasMaxLength(caseTable.UserNameField.MaxLength!.Value);

            entity.Property(x => x.PaymentIndicator)
                .HasColumnName(caseTable.PaymentIndicatorField.Name)
                .HasMaxLength(caseTable.PaymentIndicatorField.MaxLength!.Value);

            entity.Property(x => x.Status)
                .HasColumnName(caseTable.StatusField.Name)
                .HasMaxLength(caseTable.StatusField.MaxLength!.Value);

            entity.Property(x => x.ApprovalIndicator)
                .HasColumnName(caseTable.ApprovalIndicatorField.Name)
                .HasMaxLength(caseTable.ApprovalIndicatorField.MaxLength!.Value);

            entity.Property(x => x.ApprovingClerk)
                .HasColumnName(caseTable.ApprovingClerkField.Name)
                .HasMaxLength(caseTable.ApprovingClerkField.MaxLength!.Value);

            entity.Property(x => x.ApproveDate)
                .HasColumnName(caseTable.ApproveDateField.Name)
                .HasColumnType(caseTable.ApproveDateField.ColumnType);

            entity.Property(x => x.FeePaidDate)
                .HasColumnName(caseTable.FeePaidDateField.Name)
                .HasColumnType(caseTable.FeePaidDateField.ColumnType);

            entity.Property(x => x.FeePaymentType)
                .HasColumnName(caseTable.FeePaymentTypeField.Name)
                .HasMaxLength(caseTable.FeePaymentTypeField.MaxLength!.Value);

            entity.Property(x => x.FeePaidAmount)
                .HasColumnName(caseTable.FeePaidAmountField.Name)
                .HasPrecision
                (
                    caseTable.FeePaidAmountField.Precision!.Value,
                    caseTable.FeePaidAmountField.Scale!.Value
                );

            entity.Property(x => x.Narrative)
                .HasColumnName(caseTable.NarrativeField.Name)
                .HasMaxLength(caseTable.NarrativeField.MaxLength!.Value);

            entity.Property(x => x.UserId)
                .HasColumnName(caseTable.UserIdField.Name)
                .HasMaxLength(caseTable.UserIdField.MaxLength!.Value);

            entity.Property(x => x.UserReferenceNumber)
                .HasColumnName(caseTable.UserReferenceNumberField.Name)
                .HasMaxLength(caseTable.UserReferenceNumberField.MaxLength!.Value);

            entity.Property(x => x.EFilingIndicator)
                .HasColumnName(caseTable.EFilingIndicatorField.Name)
                .HasMaxLength(caseTable.EFilingIndicatorField.MaxLength!.Value);
        });
    }

    private void BuildCarts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable(cartTable.Name);

            entity.HasKey(x => x.TransactionId);

            entity.Property(x => x.TransactionId)
                .HasColumnName(cartTable.TransactionIdField.Name)
                .HasMaxLength(cartTable.TransactionIdField.MaxLength!.Value);

            entity.Property(x => x.PurchaseType)
                .HasColumnName(cartTable.PurchaseTypeField.Name)
                .HasMaxLength(cartTable.PurchaseTypeField.MaxLength!.Value);

            entity.Property(x => x.UserName)
                .HasColumnName(cartTable.UserNameField.Name)
                .HasMaxLength(cartTable.UserNameField.MaxLength!.Value);

            entity.Property(x => x.ConfirmationNumber)
                .HasColumnName(cartTable.ConfirmationNumberField.Name)
                .HasMaxLength(cartTable.ConfirmationNumberField.MaxLength!.Value);

            entity.Property(x => x.Email)
                .HasColumnName(cartTable.EmailField.Name)
                .HasMaxLength(cartTable.EmailField.MaxLength!.Value);

            entity.Property(x => x.AdditionalEmail1)
                .HasColumnName(cartTable.AdditionalEmail1Field.Name)
                .HasMaxLength(cartTable.AdditionalEmail1Field.MaxLength!.Value);

            entity.Property(x => x.AdditionalEmail2)
                .HasColumnName(cartTable.AdditionalEmail2Field.Name)
                .HasMaxLength(cartTable.AdditionalEmail2Field.MaxLength!.Value);

            entity.Property(x => x.CreditCardType)
                .HasColumnName(cartTable.CreditCardTypeField.Name)
                .HasMaxLength(cartTable.CreditCardTypeField.MaxLength!.Value);

            entity.Property(x => x.PaymentNetworkReference)
                .HasColumnName(cartTable.PaymentNetworkReferenceField.Name)
                .HasMaxLength(cartTable.PaymentNetworkReferenceField.MaxLength!.Value);

            entity.Property(x => x.NumberOfItems)
                .HasColumnName(cartTable.NumberOfItemsField.Name)
                .HasPrecision
                (
                    cartTable.NumberOfItemsField.Precision!.Value,
                    cartTable.NumberOfItemsField.Scale!.Value
                );

            entity.Property(x => x.TotalPages)
                .HasColumnName(cartTable.TotalPagesField.Name)
                .HasPrecision
                (
                    cartTable.TotalPagesField.Precision!.Value,
                    cartTable.TotalPagesField.Scale!.Value
                );

            entity.Property(x => x.TotalPrice)
                .HasColumnName(cartTable.TotalPriceField.Name)
                .HasPrecision
                (
                    cartTable.TotalPriceField.Precision!.Value,
                    cartTable.TotalPriceField.Scale!.Value
                );

            entity.Property(x => x.InitiatedDate)
                .HasColumnName(cartTable.InitiatedDateField.Name)
                .HasColumnType(cartTable.InitiatedDateField.ColumnType!);

            entity.Property(x => x.Status)
                .HasColumnName(cartTable.StatusField.Name)
                .HasMaxLength(cartTable.StatusField.MaxLength!.Value);

            entity.Property(x => x.PaidDate)
                .HasColumnName(cartTable.PaidDateField.Name)
                .HasColumnType(cartTable.PaidDateField.ColumnType!);
        });
    }

    private void BuildUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(userTable.Name);

            entity.HasKey(x => x.UserName);

            entity.HasIndex(x => x.UserName)
                .HasDatabaseName(userTable.UniqueIndex)
                .IsUnique();

            entity.Property(x => x.UserName)
                .HasColumnName(userTable.UserNameField.Name)
                .HasMaxLength(userTable.UserNameField.MaxLength!.Value);

            entity.Property(x => x.Password)
                .HasColumnName(userTable.PasswordField.Name)
                .HasMaxLength(userTable.PasswordField.MaxLength!.Value);

            entity.Property(x => x.Pin)
                .HasColumnName(userTable.PinField.Name)
                .HasMaxLength(userTable.PinField.MaxLength!.Value);

            entity.Property(x => x.UserType)
                .HasColumnName(userTable.UserTypeField.Name)
                .HasMaxLength(userTable.UserTypeField.MaxLength!.Value);

            entity.Property(x => x.UserStatus)
                .HasColumnName(userTable.UserStatusField.Name)
                .HasMaxLength(userTable.UserStatusField.MaxLength!.Value);

            entity.Property(x => x.ExpirationDate)
                .HasColumnName(userTable.ExpirationDateField.Name)
                .HasColumnType(userTable.ExpirationDateField.ColumnType!);

            entity.Property(x => x.EntryDate)
                .HasColumnName(userTable.EntryDateField.Name)
                .HasColumnType(userTable.EntryDateField.ColumnType!);

            entity.Property(x => x.AttorneyId)
                .HasColumnName(userTable.AttorneyIdField.Name)
                .HasMaxLength(userTable.AttorneyIdField.MaxLength!.Value);

            entity.Property(x => x.ProSeNumber)
                .HasColumnName(userTable.ProSeNumberField.Name)
                .HasMaxLength(userTable.ProSeNumberField.MaxLength!.Value);

            entity.Property(x => x.CityLawIndicator)
                .HasColumnName(userTable.CityLawIndicatorField.Name)
                .HasMaxLength(userTable.CityLawIndicatorField.MaxLength!.Value);

            entity.Property(x => x.ParticipantId)
                .HasColumnName(userTable.ParticipantIdField.Name)
                .HasPrecision
                (
                    userTable.ParticipantIdField.Precision!.Value,
                    userTable.ParticipantIdField.Scale!.Value
                );
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        BuildCases(modelBuilder);

        BuildCarts(modelBuilder);

        BuildUsers(modelBuilder);
    }
}