namespace CivilSubpoenaAccess.Api.Database.Schema.Tables;


public interface ICartTable
{
    string Name { get; }

    FieldInfo TransactionIdField { get; }

    FieldInfo PurchaseTypeField { get; }

    FieldInfo UserNameField { get; }

    FieldInfo ConfirmationNumberField { get; }

    FieldInfo EmailField { get; }

    FieldInfo AdditionalEmail1Field { get; }

    FieldInfo AdditionalEmail2Field { get; }

    FieldInfo CreditCardTypeField { get; }

    FieldInfo PaymentNetworkReferenceField { get; }

    FieldInfo NumberOfItemsField { get; }

    FieldInfo TotalPagesField { get; }

    FieldInfo TotalPriceField { get; }

    FieldInfo InitiatedDateField { get; }

    FieldInfo StatusField { get; }

    FieldInfo PaidDateField { get; }
}

public class CartTable : ICartTable
{
    public string Name => "ZISCART";

    public FieldInfo TransactionIdField { get; init; } = new()
    {
        Name = "ZISCART_TID",

        MaxLength = 10,

        Nullable = true
    };

    public FieldInfo PurchaseTypeField { get; init; } = new()
    {
        Name = "ZISCART_PURCHASE_TYPE",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo UserNameField { get; init; } = new()
    {
        Name = "ZISCART_USERNAME",

        MaxLength = 15,

        Nullable = true
    };

    public FieldInfo ConfirmationNumberField { get; init; } = new()
    {
        Name = "ZISCART_CONFIRMATION_NO",

        MaxLength = 15,

        Nullable = true
    };

    public FieldInfo EmailField { get; init; } = new()
    {
        Name = "ZISCART_EMAIL",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo AdditionalEmail1Field { get; init; } = new()
    {
        Name = "ZISCART_ADDL_EMAIL1",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo AdditionalEmail2Field { get; init; } = new()
    {
        Name = "ZISCART_ADDL_EMAIL2",

        MaxLength = 60,

        Nullable = true
    };

    public FieldInfo CreditCardTypeField { get; init; } = new()
    {
        Name = "ZISCART_CC_TYPE",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo PaymentNetworkReferenceField { get; init; } = new()
    {
        Name = "ZISCART_PNREF",

        MaxLength = 15,

        Nullable = true
    };

    public FieldInfo NumberOfItemsField { get; init; } = new()
    {
        Name = "ZISCART_NO_ITEMS",

        Precision = 3,

        Scale = 0,

        Nullable = true
    };

    public FieldInfo TotalPagesField { get; init; } = new()
    {
        Name = "ZISCART_TOTAL_PAGES",

        Precision = 3,

        Scale = 0,

        Nullable = true
    };

    public FieldInfo TotalPriceField { get; init; } = new()
    {
        Name = "ZISCART_TOTAL_PRICE",

        Precision = 6,

        Scale = 2,

        Nullable = true
    };

    public FieldInfo InitiatedDateField { get; init; } = new()
    {
        Name = "ZISCART_INIT_DATE",

        ColumnType = "DATE",

        Nullable = true
    };

    public FieldInfo StatusField { get; init; } = new()
    {
        Name = "ZISCART_STATUS",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo PaidDateField { get; init; } = new()
    {
        Name = "ZISCART_PAID_DATE",

        ColumnType = "DATE",

        Nullable = true
    };
}