using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

using CivilSubpoenaAccess.Api.Database;
using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Serialization;
using CivilSubpoenaAccess.Api.Reports.Enums;
using CivilSubpoenaAccess.Api.Reports.Filters;
using CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;

namespace CivilSubpoenaAccess.Api.Reports.Queries.LineItems;


public interface ITransactionsBySubpoenaInfo
{
    Task<Result<TransactionsBySubpoenaType, DatabaseError>> TransactionsBySubpoenaType
    (
        SubpoenaTypeFilters subpoenaTypeFilters
    );

    Task<Result<TransactionsBySubpoenaStatus, DatabaseError>> TransactionsBySubpoenaStatus
    (
        SubpoenaStatusFilters subpoenaStatusFilters
    );
}

public class TransactionsBySubpoenaInfo
(
    IDbContextFactory<EFilingDatabaseContext> dbContextFactory,

    IHttpContextAccessor httpContextAccessor,

    ILogger serilogger
)
    : IndentingFormatter, ITransactionsBySubpoenaInfo
{
    private void ExecutingTransactionsBySubpoenaType()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(TransactionsBySubpoenaType));
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

    private static IQueryable<TransactionInfo> TransactionsBySubpoenaType
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

                ApprovedDate = group.Max(x => x.Case.ApproveDate),

                RejectedDate = null,

                ItemCount = group.Count(),

                PaymentAmount = group.Max(x => x.Cart.TotalPrice) ?? 0m
            })

            .Distinct()

            .OrderBy(x => x.TransactionId);
    }

    public async Task<Result<TransactionsBySubpoenaType, DatabaseError>> TransactionsBySubpoenaType
    (
        SubpoenaTypeFilters subpoenaTypeFilters
    )
    {
        try
        {
            ExecutingTransactionsBySubpoenaType();

            return new TransactionsBySubpoenaType
            {
                Attend = TransactionsBySubpoenaType(await DbContext(), subpoenaTypeFilters.Attend),

                Produce = TransactionsBySubpoenaType(await DbContext(), subpoenaTypeFilters.Produce)
            };
        }
        catch (Exception exception)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to load transactions by subpoena type",

                Exception = exception
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
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

                    group.First().Case.ApprovalIndicator == ApprovalIndicator.Rejected

                        ? group.Max(x => x.Case.ApproveDate)

                        : null,

                ItemCount = group.Count(),

                PaymentAmount = group.Max(x => x.Cart.TotalPrice) ?? 0m
            })

            .Distinct()

            .OrderBy(x => x.TransactionId);
    }

    private void ExecutingTransactionsBySubpoenaStatus()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(TransactionsBySubpoenaStatus));
    }

    public async Task<Result<TransactionsBySubpoenaStatus, DatabaseError>> TransactionsBySubpoenaStatus
    (
        SubpoenaStatusFilters subpoenaStatusFilters
    )
    {
        try
        {
            ExecutingTransactionsBySubpoenaStatus();

            return new TransactionsBySubpoenaStatus
            {
                Approved = Transactions(await DbContext(), subpoenaStatusFilters.Approved),

                Rejected = Transactions(await DbContext(), subpoenaStatusFilters.Rejected),

                PendingApproval = Transactions(await DbContext(), subpoenaStatusFilters.PendingApproval),

                PendingPayment = Transactions(await DbContext(), subpoenaStatusFilters.PendingPayment)
            };
        }
        catch (Exception transactionsException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Error executing {nameof(TransactionsBySubpoenaStatus)} query",

                Exception = transactionsException
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
    }
}