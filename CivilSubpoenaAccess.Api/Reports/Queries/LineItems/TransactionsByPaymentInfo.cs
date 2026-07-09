using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Database;
using CivilSubpoenaAccess.Api.Reports.Enums;
using CivilSubpoenaAccess.Api.Reports.Filters;
using CivilSubpoenaAccess.Api.Serialization;
using CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;
using CivilSubpoenaAccess.Api.Reports.DataModels;

namespace CivilSubpoenaAccess.Api.Reports.Queries.LineItems;


public interface ITransactionsByPaymentInfo
{
    Task<Result<TransactionsByPaymentType, DatabaseError>> TransactionsByPaymentType
    (
        PaymentTypeCountFilters paymentTypeCountFilters
    );
}

public class TransactionsByPaymentInfo
(
    IDbContextFactory<EFilingDatabaseContext> dbContextFactory,

    IHttpContextAccessor httpContextAccessor,

    ILogger serilogger
)
    : IndentingFormatter, ITransactionsByPaymentInfo
{
    private void ExecutingTransactionsByUserType()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(TransactionsByPaymentType));
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

    private static IQueryable<TransactionInfo> Transactions
    (
        EFilingDatabaseContext efilingDbContext,

        CaseFilter caseFilter
    )
    {
        return efilingDbContext.Cases

            .AsNoTracking()

            .Where(caseFilter)

            .Join
            (
                efilingDbContext.Carts.AsNoTracking(),

                @case => @case.TransactionId,

                cart => cart.TransactionId,

                (@case, cart) => new
                {
                    Case = @case,

                    Cart = cart
                }
            )

            .GroupBy(x => x.Case.TransactionId)

            .Select(group => new TransactionInfo
            {
                TransactionId = group.Key,

                Username = group.Max(x => x.Case.UserName),

                FilingDate = group.Max(x => x.Case.DateFiled),

                ApprovedDate = group.Max(x => x.Case.ApproveDate),

                RejectedDate =

                    group.Max(x => x.Case.ApprovalIndicator) == ApprovalIndicator.Rejected

                        ? group.Max(x => x.Case.ApproveDate)

                        : null,

                ItemCount = group.Count(),

                PaymentAmount = group.Max(x => x.Cart.TotalPrice) ?? 0m
            })

            .Distinct()

            .OrderBy(x => x.TransactionId);
    }

    private static IQueryable<TransactionInfo> Transactions
    (
        EFilingDatabaseContext efilingDbContext,

        CaseCartFilter caseCartFilter
    )
    {
        return efilingDbContext.Cases

            .AsNoTracking()

            .Join
            (
                efilingDbContext.Carts.AsNoTracking(),

                @case => @case.TransactionId,

                cart => cart.TransactionId,

                (@case, cart) => new
                {
                    Case = @case,

                    Cart = cart
                }
            )

            .Select(x => new CaseCart
            {
                Case = x.Case,

                Cart = x.Cart
            })

            .Where(caseCartFilter)

            .GroupBy(x => x.Case.TransactionId)

            .Select(group => new TransactionInfo
            {
                TransactionId = group.Key,

                Username = group.Max(x => x.Case.UserName),

                FilingDate = group.Max(x => x.Case.DateFiled),

                ApprovedDate = group.Max(x => x.Case.ApproveDate),

                RejectedDate =

                    group.Max(x => x.Case.ApprovalIndicator) == ApprovalIndicator.Rejected

                        ? group.Max(x => x.Case.ApproveDate)

                        : null,

                ItemCount = group.Count(),

                PaymentAmount = group.Max(x => x.Cart.TotalPrice) ?? 0m
            })

            .Distinct()

            .OrderBy(x => x.TransactionId);
    }

    public async Task<Result<TransactionsByPaymentType, DatabaseError>> TransactionsByPaymentType
    (
        PaymentTypeCountFilters paymentTypeCountFilters
    )
    {
        try
        {
            ExecutingTransactionsByUserType();

            return new TransactionsByPaymentType
            {
                InFormaPauperis =

                    Transactions(await DbContext(), paymentTypeCountFilters.InFormaPauperis),

                WalkIn =

                    Transactions(await DbContext(), paymentTypeCountFilters.WalkIn),

                CityLaw =

                    Transactions(await DbContext(), paymentTypeCountFilters.CityLaw),

                CitySolicitor =

                    Transactions(await DbContext(), paymentTypeCountFilters.CitySolicitor),

                AmericanExpress =

                    Transactions(await DbContext(), paymentTypeCountFilters.AmericanExpress),

                DiscoverCard =

                    Transactions(await DbContext(), paymentTypeCountFilters.DiscoverCard),

                Mastercard =

                    Transactions(await DbContext(), paymentTypeCountFilters.Mastercard),

                VisaCard =

                    Transactions(await DbContext(), paymentTypeCountFilters.VisaCard)
            };
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Error executing {nameof(TransactionsByPaymentType)} query",

                Exception = databaseException
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
    }
}