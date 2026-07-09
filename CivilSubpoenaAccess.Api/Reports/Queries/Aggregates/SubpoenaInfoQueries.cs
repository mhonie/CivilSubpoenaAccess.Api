using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

using CivilSubpoenaAccess.Api.Database;
using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Serialization;
using CivilSubpoenaAccess.Api.Reports.Filters;
using CivilSubpoenaAccess.Api.Reports.DataModels.Aggregates;

namespace CivilSubpoenaAccess.Api.Reports.Queries.Aggregates;


public interface ISubpoenaInfoQueries
{
    Task<Result<SubpoenaStatusCounts, DatabaseError>> SubpoenaStatusCounts
    (
        SubpoenaStatusFilters subpoenaStatusFilters
    );

    Task<Result<SubpoenaTypeCounts, DatabaseError>> SubpoenaTypeCounts
    (
        SubpoenaTypeFilters subpoenaTypeFilters
    );
}

public class SubpoenaInfoQueries
(
    IDbContextFactory<EFilingDatabaseContext> efilingDbContextFactory,

    ILogger serilogger
)
    : IndentingFormatter, ISubpoenaInfoQueries
{
    private void ExecutingSubpoenaStatusCounts()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(SubpoenaStatusCounts));
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

    public async Task<Result<SubpoenaStatusCounts, DatabaseError>> SubpoenaStatusCounts
    (
        SubpoenaStatusFilters subpoenaStatusFilters
    )
    {
        try
        {
            ExecutingSubpoenaStatusCounts();

            await using var efilingDbContext = 
                
                await efilingDbContextFactory.CreateDbContextAsync();

            int approvedCount = await efilingDbContext.Cases

                    .AsNoTracking()

                    .CountAsync(subpoenaStatusFilters.Approved);

            int rejectedCount = await efilingDbContext.Cases

                    .AsNoTracking()

                    .CountAsync(subpoenaStatusFilters.Rejected);

            int pendingPaymentCount = await efilingDbContext.Cases

                    .AsNoTracking()

                    .CountAsync(subpoenaStatusFilters.PendingPayment);

            int pendingApprovalCount = await efilingDbContext.Cases

                    .AsNoTracking()

                    .CountAsync(subpoenaStatusFilters.PendingApproval);

            return new SubpoenaStatusCounts
            {
                Approved = approvedCount,

                Rejected = rejectedCount,

                PendingPayment = pendingPaymentCount,

                PendingApproval = pendingApprovalCount
            };
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Error executing {nameof(SubpoenaStatusCounts)} query",

                Exception = databaseException
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
    }

    private void ExecutingSubpoenaTypeCounts()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(SubpoenaTypeCounts));
    }

    public async Task<Result<SubpoenaTypeCounts, DatabaseError>> SubpoenaTypeCounts
    (
        SubpoenaTypeFilters subpoenaTypeFilters
    )
    {
        try
        {
            ExecutingSubpoenaTypeCounts();

            await using var efilingDbContext =

                await efilingDbContextFactory.CreateDbContextAsync();

            int attend = await efilingDbContext.Cases

                .AsNoTracking()

                .CountAsync(subpoenaTypeFilters.Attend);

            int produce = await efilingDbContext.Cases

                .AsNoTracking()

                .CountAsync(subpoenaTypeFilters.Produce);

            return new SubpoenaTypeCounts
            {
                Attend = attend,

                Produce = produce
            };
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Error executing {nameof(SubpoenaTypeCounts)} query",

                Exception = databaseException
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
    }
}