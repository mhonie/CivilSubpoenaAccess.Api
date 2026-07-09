using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

using CivilSubpoenaAccess.Api.Database;
using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Serialization;
using CivilSubpoenaAccess.Api.Reports.UserTypes;
using CivilSubpoenaAccess.Api.Reports.DataModels.Aggregates;

namespace CivilSubpoenaAccess.Api.Reports.Queries.Aggregates;


public interface IUserInfoQueries
{
    Task<Result<UserTypeCounts, DatabaseError>> UserTypeCounts
    (
        CaseFilter approvedSubpoenaFilter
    );
}

public class UserInfoQueries
(
    IUserTypes userTypes,

    IDbContextFactory<EFilingDatabaseContext> efilingDbContextFactory,

    ILogger serilogger
)
    : IndentingFormatter, IUserInfoQueries
{
    private void ExecutingUserTypeCounts()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(UserTypeCounts));
    }

    private void ErrorExecutingQuery(DatabaseError databaseError)
    {
        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error
        (
            databaseError.Exception,

            "{databaseErrorMessage}",

            databaseErrorJson
        );
    }
    
    public async Task<Result<UserTypeCounts, DatabaseError>> UserTypeCounts
    (
        CaseFilter approvedSubpoenaFilter
    )
    {
        try
        {
            ExecutingUserTypeCounts();

            await using var efilingDbContext =

                await efilingDbContextFactory.CreateDbContextAsync();

            var acceptedCases = efilingDbContext.Cases

                .AsNoTracking()

                .Where(approvedSubpoenaFilter);

            var userDataRows = await
            (
                from @case in acceptedCases

                join cart in efilingDbContext.Carts

                    on @case.TransactionId equals cart.TransactionId

                    into cartJoin

                from cart in cartJoin.DefaultIfEmpty()

                join user in efilingDbContext.Users

                    on @case.UserName equals user.UserName

                    into userJoin

                from user in userJoin.DefaultIfEmpty()

                select new UserData
                {
                    UserName = @case.UserName,

                    PaymentIndicator = @case.PaymentIndicator,

                    CreditCardType = cart.CreditCardType,

                    ProSeNumber = user.ProSeNumber
                }
            )
                .ToListAsync();

            var publicCount = 0;

            var eFilingAttorneyCount = 0;

            var eFilingProSeCount = 0;

            var cityLawCount = 0;

            var citySolicitorCount = 0;

            var filingServiceCount = 0;

            foreach (var userData in userDataRows)
            {
                if (userTypes.IsPublicUser(userData))
                {
                    publicCount++;
                }
                else if (userTypes.IsFilingServiceUser(userData))
                {
                    filingServiceCount++;
                }
                else if (userTypes.IsProSeUser(userData))
                {
                    eFilingProSeCount++;
                }
                else if (userTypes.IsCityLawUser(userData))
                {
                    cityLawCount++;
                }
                else if (userTypes.IsCitySolicitorUser(userData))
                {
                    citySolicitorCount++;
                }
                else
                {
                    eFilingAttorneyCount++;
                }
            }

            return new UserTypeCounts
            {
                Public = publicCount,

                EFilingAttorney = eFilingAttorneyCount,

                EFilingProSe = eFilingProSeCount,

                CityLaw = cityLawCount,

                CitySolicitor = citySolicitorCount,

                FilingService = filingServiceCount
            };
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Error executing {nameof(UserTypeCounts)} query",

                Exception = databaseException
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
    }
}