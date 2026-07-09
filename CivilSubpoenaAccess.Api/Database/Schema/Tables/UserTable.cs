namespace CivilSubpoenaAccess.Api.Database.Schema.Tables;


public interface IUserTable
{
    string Name { get; }

    string UniqueIndex { get; }

    FieldInfo UserNameField { get; }

    FieldInfo PasswordField { get; }

    FieldInfo PinField { get; }

    FieldInfo UserTypeField { get; }

    FieldInfo UserStatusField { get; }

    FieldInfo ExpirationDateField { get; }

    FieldInfo EntryDateField { get; }

    FieldInfo AttorneyIdField { get; }

    FieldInfo ProSeNumberField { get; }

    FieldInfo CityLawIndicatorField { get; }

    FieldInfo ParticipantIdField { get; }
}

public class UserTable : IUserTable
{
    public string Name => "ZIRUSER";

    public string UniqueIndex => "UK1_ZIRUSER";

    public FieldInfo UserNameField { get; init; } = new()
    {
        Name = "ZIRUSER_USERNAME",

        MaxLength = 15,

        Nullable = true
    };

    public FieldInfo PasswordField { get; init; } = new()
    {
        Name = "ZIRUSER_PASSWD",

        MaxLength = 15,

        Nullable = true
    };

    public FieldInfo PinField { get; init; } = new()
    {
        Name = "ZIRUSER_PIN",

        MaxLength = 6,

        Nullable = true
    };

    public FieldInfo UserTypeField { get; init; } = new()
    {
        Name = "ZIRUSER_USER_TYPE",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo UserStatusField { get; init; } = new()
    {
        Name = "ZIRUSER_USER_STATUS",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo ExpirationDateField { get; init; } = new()
    {
        Name = "ZIRUSER_EXP_DATE",

        ColumnType = "DATE",

        Nullable = true
    };

    public FieldInfo EntryDateField { get; init; } = new()
    {
        Name = "ZIRUSER_ENTRY_DATE",

        ColumnType = "DATE",

        Nullable = true
    };

    public FieldInfo AttorneyIdField { get; init; } = new()
    {
        Name = "ZIRUSER_ATTY_ID",

        MaxLength = 9,

        Nullable = true
    };

    public FieldInfo ProSeNumberField { get; init; } = new()
    {
        Name = "ZIRUSER_PROSE_NO",

        MaxLength = 9,

        Nullable = true
    };

    public FieldInfo CityLawIndicatorField { get; init; } = new()
    {
        Name = "ZIRUSER_CITY_LAW_IND",

        MaxLength = 1,

        Nullable = true
    };

    public FieldInfo ParticipantIdField { get; init; } = new()
    {
        Name = "ZIRUSER_PIDM",

        Precision = 8,

        Scale = 0,

        Nullable = true
    };
}