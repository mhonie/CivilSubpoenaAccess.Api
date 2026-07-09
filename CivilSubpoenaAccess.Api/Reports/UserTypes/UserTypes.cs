using CivilSubpoenaAccess.Api.Reports.Enums;

namespace CivilSubpoenaAccess.Api.Reports.UserTypes;


public class UserData
{
    public string? UserName { get; init; }

    public string? PaymentIndicator { get; init; }

    public string? CreditCardType { get; init; }

    public string? ProSeNumber { get; init; }
}

public interface IUserTypes
{
    public bool IsPublicUser(UserData userData);

    public bool IsFilingServiceUser(UserData userData);

    public bool IsProSeUser(UserData userData);

    public bool IsCityLawUser(UserData userData);

    public bool IsCitySolicitorUser(UserData userData);
}

public class UserTypes : IUserTypes
{
    private const string PublicUserPattern = "shopper";

    private const string FilingServiceUserPattern = "srvu";

    public bool IsPublicUser(UserData userData)
    {
        string userName = userData.UserName ?? string.Empty;

        return userName.StartsWith
        (
            PublicUserPattern,

            StringComparison.OrdinalIgnoreCase
        );
    }

    public bool IsFilingServiceUser(UserData userData)
    {
        string userName = userData.UserName ?? string.Empty;

        return userName.StartsWith
        (
            FilingServiceUserPattern,

            StringComparison.OrdinalIgnoreCase
        );
    }

    public bool IsProSeUser(UserData userData)
    {
        return !string.IsNullOrWhiteSpace(userData.ProSeNumber);
    }

    public bool IsCityLawUser(UserData userData)
    {
        return userData.PaymentIndicator == PaymentIndicator.CityLaw;
    }

    public bool IsCitySolicitorUser(UserData userData)
    {
        return userData.PaymentIndicator == PaymentIndicator.CitySolicitor

               && string.IsNullOrWhiteSpace(userData.CreditCardType);
    }
}