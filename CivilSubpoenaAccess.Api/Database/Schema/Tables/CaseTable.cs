namespace CivilSubpoenaAccess.Api.Database.Schema.Tables;


public interface ICaseTable
{
    string Name { get; }

    string UniqueIndex { get; }

    FieldInfo TransactionIdField { get; }

    FieldInfo SubpoenaNumberField { get; }

    FieldInfo CaseIdField { get; }

    FieldInfo SubpoenaTypeField { get; }

    FieldInfo AddresseeField { get; }

    FieldInfo Address1Field { get; }

    FieldInfo Address2Field { get; }

    FieldInfo Address3Field { get; }

    FieldInfo CityField { get; }

    FieldInfo StateField { get; }

    FieldInfo ZipCodeField { get; }

    FieldInfo EventDateField { get; }

    FieldInfo LocationField { get; }

    FieldInfo DateFiledField { get; }

    FieldInfo EventTimeField { get; }

    FieldInfo TestifyField { get; }

    FieldInfo EvidenceField { get; }

    FieldInfo AttorneyField { get; }

    FieldInfo AttorneyAddress1Field { get; }

    FieldInfo AttorneyAddress2Field { get; }

    FieldInfo AttorneyAddress3Field { get; }

    FieldInfo AttorneyTelephoneField { get; }

    FieldInfo AttorneyIdField { get; }

    FieldInfo AttorneySubpoenaIdField { get; }

    FieldInfo AttorneyForField { get; }

    FieldInfo EmailField { get; }

    FieldInfo ConfirmationNumberField { get; }

    FieldInfo UserNameField { get; }

    FieldInfo PaymentIndicatorField { get; }

    FieldInfo StatusField { get; }

    FieldInfo ApprovalIndicatorField { get; }

    FieldInfo ApprovingClerkField { get; }

    FieldInfo ApproveDateField { get; }

    FieldInfo FeePaidDateField { get; }

    FieldInfo FeePaymentTypeField { get; }

    FieldInfo FeePaidAmountField { get; }

    FieldInfo NarrativeField { get; }

    FieldInfo UserIdField { get; }

    FieldInfo UserReferenceNumberField { get; }

    FieldInfo EFilingIndicatorField { get; }
}

public class CaseTable : ICaseTable
{
    public string Name => "ZISCASE";

    public string UniqueIndex => "UK_ZISCASE";

    public FieldInfo TransactionIdField { get; init; } = new()
    {
        Name = "ZISCASE_TID",

        MaxLength = 10,

        Nullable = true
    };

    public FieldInfo SubpoenaNumberField { get; init; } = new()
    {
        Name = "ZISCASE_SUBPOENA_NO",

        Precision = 3,

        Scale = 0,

        Nullable = true
    };

    public FieldInfo CaseIdField { get; init; } = new()
    {
        Name = "ZISCASE_CASEID",

        MaxLength = 25,

        Nullable = true
    };

    public FieldInfo SubpoenaTypeField { get; init; } = new()
    {
        Name = "ZISCASE_TYPE",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo AddresseeField { get; init; } = new()
    {
        Name = "ZISCASE_TO",

        MaxLength = 100,

        Nullable = true
    };

    public FieldInfo Address1Field { get; init; } = new()
    {
        Name = "ZISCASE_ADDRESS1",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo Address2Field { get; init; } = new()
    {
        Name = "ZISCASE_ADDRESS2",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo Address3Field { get; init; } = new()
    {
        Name = "ZISCASE_ADDRESS3",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo CityField { get; init; } = new()
    {
        Name = "ZISCASE_CITY",

        MaxLength = 30,

        Nullable = true
    };

    public FieldInfo StateField { get; init; } = new()
    {
        Name = "ZISCASE_STATE",

        MaxLength = 2,

        Nullable = true
    };

    public FieldInfo ZipCodeField { get; init; } = new()
    {
        Name = "ZISCASE_ZIPCODE",

        MaxLength = 30,

        Nullable = true
    };

    public FieldInfo EventDateField { get; init; } = new()
    {
        Name = "ZISCASE_ONDATE",

        ColumnType = "DATE",

        Nullable = true
    };

    public FieldInfo LocationField { get; init; } = new()
    {
        Name = "ZISCASE_LOCATION",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo DateFiledField { get; init; } = new()
    {
        Name = "ZISCASE_DATE_FILED",

        ColumnType = "DATE",

        Nullable = true
    };

    public FieldInfo EventTimeField { get; init; } = new()
    {
        Name = "ZISCASE_TIME",

        MaxLength = 6,

        Nullable = true
    };

    public FieldInfo TestifyField { get; init; } = new()
    {
        Name = "ZISCASE_TESTIFY",

        MaxLength = 100,

        Nullable = true
    };

    public FieldInfo EvidenceField { get; init; } = new()
    {
        Name = "ZISCASE_EVIDENCE",

        MaxLength = 440,

        Nullable = true
    };

    public FieldInfo AttorneyField { get; init; } = new()
    {
        Name = "ZISCASE_ATTORNEY",

        MaxLength = 100,

        Nullable = true
    };

    public FieldInfo AttorneyAddress1Field { get; init; } = new()
    {
        Name = "ZISCASE_ATTORNEY_ADDRESS1",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo AttorneyAddress2Field { get; init; } = new()
    {
        Name = "ZISCASE_ATTORNEY_ADDRESS2",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo AttorneyAddress3Field { get; init; } = new()
    {
        Name = "ZISCASE_ATTORNEY_ADDRESS3",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo AttorneyTelephoneField { get; init; } = new()
    {
        Name = "ZISCASE_ATTORNEY_TELEPHONE",

        MaxLength = 15,

        Nullable = true
    };

    public FieldInfo AttorneyIdField { get; init; } = new()
    {
        Name = "ZISCASE_ATTORNEY_ID",

        MaxLength = 10,

        Nullable = true
    };

    public FieldInfo AttorneySubpoenaIdField { get; init; } = new()
    {
        Name = "ZISCASE_ATTORNEY_SID",

        MaxLength = 10,

        Nullable = true
    };

    public FieldInfo AttorneyForField { get; init; } = new()
    {
        Name = "ZISCASE_ATTORNEY_FOR",

        MaxLength = 30,

        Nullable = true
    };

    public FieldInfo EmailField { get; init; } = new()
    {
        Name = "ZISCASE_EMAIL",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo ConfirmationNumberField { get; init; } = new()
    {
        Name = "ZISCASE_CONFIRMATION_NO",

        MaxLength = 15,

        Nullable = true
    };

    public FieldInfo UserNameField { get; init; } = new()
    {
        Name = "ZISCASE_USERNAME",

        MaxLength = 15,

        Nullable = true
    };

    public FieldInfo PaymentIndicatorField { get; init; } = new()
    {
        Name = "ZISCASE_PAYMENT_IND",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo StatusField { get; init; } = new()
    {
        Name = "ZISCASE_STATUS",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo ApprovalIndicatorField { get; init; } = new()
    {
        Name = "ZISCASE_APPROVAL_IND",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo ApprovingClerkField { get; init; } = new()
    {
        Name = "ZISCASE_APPROVING_CLERK",

        MaxLength = 15,

        Nullable = true
    };

    public FieldInfo ApproveDateField { get; init; } = new()
    {
        Name = "ZISCASE_APPROVE_DATE",

        ColumnType = "DATE",

        Nullable = true
    };

    public FieldInfo FeePaidDateField { get; init; } = new()
    {
        Name = "ZISCASE_FEE_PAID_DATE",

        ColumnType = "DATE",

        Nullable = true
    };

    public FieldInfo FeePaymentTypeField { get; init; } = new()
    {
        Name = "ZISCASE_FEE_PAYMENT_TYPE",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo FeePaidAmountField { get; init; } = new()
    {
        Name = "ZISCASE_FEE_PAID_AMT",

        Precision = 13,

        Scale = 2,

        Nullable = true
    };

    public FieldInfo NarrativeField { get; init; } = new()
    {
        Name = "ZISCASE_NARRATIVE",

        MaxLength = 250,

        Nullable = true
    };

    public FieldInfo UserIdField { get; init; } = new()
    {
        Name = "ZISCASE_UID",

        MaxLength = 30,

        Nullable = true
    };

    public FieldInfo UserReferenceNumberField { get; init; } = new()
    {
        Name = "ZISCASE_USER_REFNUM",

        MaxLength = 30,

        Nullable = true
    };

    public FieldInfo EFilingIndicatorField { get; init; } = new()
    {
        Name = "ZISCASE_EFILING_IND",

        MaxLength = 3,

        Nullable = true
    };
}