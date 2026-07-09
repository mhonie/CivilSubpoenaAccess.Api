using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Database;
using CivilSubpoenaAccess.Api.Serialization;
using CivilSubpoenaAccess.Api.Reports.Enums;
using CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;

// ReSharper disable EntityFramework.ClientSideDbFunctionCall

namespace CivilSubpoenaAccess.Api.Reports.Queries.LineItems;

using CaseFilter = CaseFilter;

public interface ITransactionsByUserInfo
{
    Task<Result<TransactionsByUserType, DatabaseError>> TransactionsByUserType
    (
        CaseFilter approvedFilter
    );
}

public class TransactionsByUserInfo
(
    IDbContextFactory<EFilingDatabaseContext> dbContextFactory,

    IHttpContextAccessor httpContextAccessor,

    ILogger serilogger
)
    : IndentingFormatter, ITransactionsByUserInfo
{
    private class UserTransactionRow
    {
        public required string TransactionId { get; init; }

        public string? UserName { get; init; }

        public DateTime? ApprovedDate { get; init; }

        public DateTime? RejectedDate { get; init; }

        public required int ItemCount { get; init; }

        public required decimal PaymentAmount { get; init; }

        public string? PaymentIndicator { get; init; }

        public string? CreditCardType { get; init; }

        public string? ProSeNumber { get; init; }
    }

    private void ExecutingTransactionsByUserType()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(TransactionsByUserType));
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

    private async Task<EFilingDatabaseContext> DbContext()
    {
        var efilingDbContext = await dbContextFactory.CreateDbContextAsync();

        httpContextAccessor.HttpContext?.Response.RegisterForDisposeAsync(efilingDbContext);

        return efilingDbContext;
    }

    private static IQueryable<UserTransactionRow> UserTransactions
    (
        EFilingDatabaseContext efilingDbContext,

        CaseFilter approvedFilter
    )
    {
        var approvedCases = efilingDbContext.Cases
            
            .AsNoTracking()
            
            .Where(approvedFilter);

        return

            from @case in approvedCases

            join cart in efilingDbContext.Carts

                on @case.TransactionId equals cart.TransactionId

                into cartJoin

            from cart in cartJoin.DefaultIfEmpty()

            join user in efilingDbContext.Users

                on @case.UserName equals user.UserName

                into userJoin

            from user in userJoin.DefaultIfEmpty()

            select new UserTransactionRow
            {
                TransactionId = @case.TransactionId,

                UserName = @case.UserName,

                ApprovedDate = @case.ApproveDate,

                RejectedDate =
                    
                    @case.ApprovalIndicator == ApprovalIndicator.Rejected

                        ? @case.ApproveDate

                        : null,

                ItemCount = cart != null

                        ? cart.NumberOfItems ?? 0

                        : 0,

                PaymentAmount = cart != null

                        ? cart.TotalPrice ?? 0

                        : 0,

                PaymentIndicator = @case.PaymentIndicator,

                CreditCardType = cart != null

                        ? cart.CreditCardType

                        : null,

                ProSeNumber = user != null

                        ? user.ProSeNumber

                        : null
            };
    }

    private static IQueryable<TransactionInfo> TransactionInfo
    (
        IQueryable<UserTransactionRow> userTransactions
    )
    {
        return userTransactions

            .Select
            (
                x => new TransactionInfo
                {
                    TransactionId = x.TransactionId,

                    Username = x.UserName,

                    ApprovedDate = x.ApprovedDate,

                    RejectedDate = x.RejectedDate,

                    ItemCount = x.ItemCount,

                    PaymentAmount = x.PaymentAmount
                }
            )
            .Distinct()

            .OrderBy(x => x.TransactionId);
    }

    public async Task<Result<TransactionsByUserType, DatabaseError>> TransactionsByUserType
    (
        CaseFilter approvedFilter
    )
    {
        try
        {
            ExecutingTransactionsByUserType();

            return new TransactionsByUserType
            {
                Public =

                    TransactionInfo
                    (
                        UserTransactions
                        (
                            await DbContext(),

                            approvedFilter
                        )
                        .Where
                        (
                            x =>

                                x.UserName != null &&

                                EF.Functions.Like(x.UserName, "shopper%")
                        )
                    ),

                FilingService =

                    TransactionInfo
                    (
                        UserTransactions
                        (
                            await DbContext(),

                            approvedFilter
                        )
                        .Where
                        (
                            x =>

                                x.UserName != null &&

                                EF.Functions.Like(x.UserName, "srvu%")
                        )
                    ),

                EFilingProSe =

                    TransactionInfo
                    (
                        UserTransactions
                        (
                            await DbContext(),

                            approvedFilter
                        )
                        .Where(x => x.ProSeNumber != null && x.ProSeNumber != "")
                    ),

                CityLaw =

                    TransactionInfo
                    (
                        UserTransactions
                        (
                            await DbContext(),

                            approvedFilter
                        )
                        .Where(x => x.PaymentIndicator == PaymentIndicator.CityLaw)
                    ),

                CitySolicitor =

                    TransactionInfo
                    (
                        UserTransactions
                            (
                                await DbContext(),

                                approvedFilter
                            )
                            .Where
                            (
                                x => x.PaymentIndicator == PaymentIndicator.CitySolicitor &&
                                (
                                    x.CreditCardType == null || x.CreditCardType == ""
                                )

                            )
                    ),

                EFilingAttorney =

                    TransactionInfo
                    (
                        UserTransactions
                        (
                            await DbContext(),

                            approvedFilter
                        )
                        .Where
                        (
                            x =>

                                !(x.UserName != null && EF.Functions.Like(x.UserName, "shopper%"))

                                && !(x.UserName != null && EF.Functions.Like(x.UserName, "srvu%"))

                                && !(x.ProSeNumber != null && x.ProSeNumber != "")

                                && !
                                (
                                    x.PaymentIndicator == PaymentIndicator.CityLaw ||
                                    
                                        x.PaymentIndicator == PaymentIndicator.CitySolicitor &&
                                        
                                        (x.CreditCardType == null || x.CreditCardType == "")
                                    
                                )
                        )
                    )
            };
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Error executing {nameof(TransactionsByUserType)} query",

                Exception = databaseException
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
    }
}