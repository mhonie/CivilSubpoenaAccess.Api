using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Serialization;
using CivilSubpoenaAccess.Api.Database.Entities;

namespace CivilSubpoenaAccess.Api.Database.Repositories;


public interface ICaseRepository
{
    Task<UnitResult<DatabaseError>> Create(Case @case);

    Task<UnitResult<DatabaseError>> Create(Case[] cases);

    Task<Result<Case, DatabaseError>> Read(string transactionId, int subpoenaNumber);

    Task<Result<Case[], DatabaseError>> Read(CaseFilter caseFilter);

    Task<UnitResult<DatabaseError>> Update(Case oldCase, Case newCase);

    Task<UnitResult<DatabaseError>> Update(Case[] oldCases, Case[] newCases);

    Task<UnitResult<DatabaseError>> Delete(string transactionId, int subpoenaNumber);

    Task<UnitResult<DatabaseError>> Delete(CaseFilter caseFilter);
}

public class CaseRepository(IDbContextFactory<EFilingDatabaseContext> dbContextFactory, ILogger serilogger)

    : IndentingFormatter, ICaseRepository
{
    private void CreatingCase(string transactionId, int subpoenaNumber)
    {
        serilogger.Debug
        (
            "Creating {0} with {1}={2}, {3}={4}...\n",

            nameof(Case),

            nameof(Case.TransactionId),

            transactionId,

            nameof(Case.SubpoenaNumber),

            subpoenaNumber
        );
    }

    private void ErrorCreatingCase(DatabaseError databaseError)
    {
        const string errorCreatingCase = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorCreatingCase, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Create(Case @case)
    {
        try
        {
            CreatingCase(@case.TransactionId, @case.SubpoenaNumber);

            await using var dbContext =

                await dbContextFactory.CreateDbContextAsync();

            await dbContext.Cases.AddAsync(@case);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to create case object",

                Case = @case,

                Exception = databaseException
            };

            ErrorCreatingCase(databaseError);

            return databaseError;
        }
    }

    private void CreatingCases()
    {
        serilogger.Debug("Creating {0} objects...\n", nameof(Case));
    }

    private void ErrorCreatingCases(DatabaseError databaseError)
    {
        const string errorCreatingCases = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorCreatingCases, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Create(Case[] cases)
    {
        try
        {
            CreatingCases();

            await using var dbContext = 
                
                await dbContextFactory.CreateDbContextAsync();

            await dbContext.Cases.AddRangeAsync(cases);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to create case objects",

                Cases = cases,

                Exception = databaseException
            };

            ErrorCreatingCases(databaseError);

            return databaseError;
        }
    }

    private void ReadingCase(string transactionId, int subpoenaNumber)
    {
        serilogger.Debug
        (
            "Reading {0} with {1}={2}, {3}={4}...\n",

            nameof(Case),

            nameof(Case.TransactionId),

            transactionId,

            nameof(Case.SubpoenaNumber),

            subpoenaNumber
        );
    }

    private void ErrorReadingCase(DatabaseError databaseError)
    {
        const string errorReadingCase = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorReadingCase, databaseErrorJson);
    }

    public async Task<Result<Case, DatabaseError>> Read(string transactionId, int subpoenaNumber)
    {
        try
        {
            ReadingCase(transactionId, subpoenaNumber);

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var @case =

                await dbContext.Cases.FirstOrDefaultAsync
                (
                    entity =>

                    entity.TransactionId == transactionId &&

                    entity.SubpoenaNumber == subpoenaNumber
                );

            if (@case != null)

                return @case;

            var notFoundError = new NotFoundError
            {
                Message = $"Object not found: {nameof(Case)} " +

                          $"with {nameof(Case.TransactionId)}={transactionId}, " +

                          $"{nameof(Case.SubpoenaNumber)}={subpoenaNumber}"
            };

            ErrorReadingCase(notFoundError);

            return notFoundError;
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Failed to read {nameof(Case)} " +

                          $"with {nameof(Case.TransactionId)}={transactionId}, " +

                          $"{nameof(Case.SubpoenaNumber)}={subpoenaNumber}",

                Exception = databaseException
            };

            ErrorReadingCase(databaseError);

            return databaseError;
        }
    }

    private void ReadingCases()
    {
        serilogger.Debug("Reading {0} table...\n", nameof(Case));
    }

    private void ErrorReadingCases(DatabaseError databaseError)
    {
        const string errorReadingCases = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorReadingCases, databaseErrorJson);
    }

    public async Task<Result<Case[], DatabaseError>> Read(CaseFilter caseFilter)
    {
        try
        {
            ReadingCases();

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var cases = await dbContext.Cases.Where(caseFilter).ToArrayAsync();

            return Result.Success<Case[], DatabaseError>(cases);
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Failed to read {nameof(Case)} objects from database",

                Exception = databaseException
            };

            ErrorReadingCases(databaseError);

            return databaseError;
        }
    }
    private void UpdatingCase(string transactionId, int subpoenaNumber)
    {
        serilogger.Debug
        (
            "Updating {0} with {1}={2}, {3}={4}...\n",

            nameof(Case),

            nameof(Case.TransactionId),

            transactionId,

            nameof(Case.SubpoenaNumber),

            subpoenaNumber
        );
    }

    private void ErrorUpdatingCase(DatabaseError databaseError)
    {
        const string errorUpdatingCase = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorUpdatingCase, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Update(Case oldCase, Case newCase)
    {
        try
        {
            string transactionId = oldCase.TransactionId;

            int subpoenaNumber = oldCase.SubpoenaNumber;

            UpdatingCase(transactionId, subpoenaNumber);

            if
            (
                oldCase.TransactionId != newCase.TransactionId ||

                oldCase.SubpoenaNumber != newCase.SubpoenaNumber
            )
            {
                var databaseError = new DatabaseError
                {
                    Message = $"{nameof(Case.TransactionId)} and " +

                              $"{nameof(Case.SubpoenaNumber)} cannot be changed",

                    Cases = [oldCase, newCase]
                };

                ErrorUpdatingCase(databaseError);

                return databaseError;
            }

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var existingCase =

                await dbContext.Cases.FirstOrDefaultAsync
                (
                    entity =>

                        entity.TransactionId == transactionId &&

                        entity.SubpoenaNumber == subpoenaNumber
                );

            if (existingCase == null)
            {
                var notFoundError = new NotFoundError
                {
                    Message = $"Object not found: {nameof(Case)} " +

                              $"with {nameof(Case.TransactionId)}={transactionId}, " +

                              $"{nameof(Case.SubpoenaNumber)}={subpoenaNumber}"
                };

                ErrorUpdatingCase(notFoundError);

                return notFoundError;
            }

            dbContext.Entry(existingCase).CurrentValues.SetValues(newCase);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Failed to update {nameof(Case)} object in database",

                Cases = [oldCase, newCase],

                Exception = databaseException
            };

            ErrorUpdatingCase(databaseError);

            return databaseError;
        }
    }

    private void UpdatingCases()
    {
        serilogger.Debug("Updating {0} objects...\n", nameof(Case));
    }

    private void ErrorUpdatingCases(DatabaseError databaseError)
    {
        const string errorUpdatingCases = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorUpdatingCases, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Update(Case[] oldCases, Case[] newCases)
    {
        try
        {
            UpdatingCases();

            if (oldCases.Length != newCases.Length)
            {
                var allCases = oldCases.Concat(newCases).ToArray();

                var databaseError = new DatabaseError
                {
                    Message = $"Inconsistent {nameof(Case)} updates",

                    Cases = allCases
                };

                ErrorUpdatingCases(databaseError);

                return databaseError;
            }

            if (oldCases.Where((@case, index) => @case.TransactionId != newCases[index].TransactionId ||

                                             @case.SubpoenaNumber != newCases[index].SubpoenaNumber).Any())
            {
                var allCases = oldCases.Concat(newCases).ToArray();

                var databaseError = new DatabaseError
                {
                    Message = $"{nameof(Case.TransactionId)} and " +

                              $"{nameof(Case.SubpoenaNumber)} cannot be changed",

                    Cases = allCases
                };

                ErrorUpdatingCases(databaseError);

                return databaseError;
            }

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.Cases.UpdateRange(newCases);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var allCases = oldCases.Concat(newCases).ToArray();

            var databaseError = new DatabaseError
            {
                Message = $"Failed to update {nameof(Case)} objects in database",

                Cases = allCases,

                Exception = databaseException
            };

            ErrorUpdatingCases(databaseError);

            return databaseError;
        }
    }

    private void DeletingCase(string transactionId, int subpoenaNumber)
    {
        serilogger.Debug
        (
            "Deleting {0} with {1}={2}, {3}={4}...\n",

            nameof(Case),

            nameof(Case.TransactionId),

            transactionId,

            nameof(Case.SubpoenaNumber),

            subpoenaNumber
        );
    }

    private void ErrorDeletingCase(DatabaseError databaseError)
    {
        const string errorDeletingCase = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorDeletingCase, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Delete(string transactionId, int subpoenaNumber)
    {
        try
        {
            DeletingCase(transactionId, subpoenaNumber);

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            int rowsAffected =

                await dbContext.Cases

                    .Where
                    (
                        entity =>

                            entity.TransactionId == transactionId &&

                            entity.SubpoenaNumber == subpoenaNumber
                    )

                    .ExecuteDeleteAsync();

            if (rowsAffected > 0)

                return UnitResult.Success<DatabaseError>();

            var notFoundError = new NotFoundError
            {
                Message = $"Object not found: {nameof(Case)} " +

                          $"with {nameof(Case.TransactionId)}={transactionId}, " +

                          $"{nameof(Case.SubpoenaNumber)}={subpoenaNumber}"
            };

            ErrorDeletingCase(notFoundError);

            return notFoundError;
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to delete object from database",

                Exception = databaseException
            };

            ErrorDeletingCase(databaseError);

            return databaseError;
        }
    }

    private void DeletingCases()
    {
        serilogger.Debug("Deleting {0} objects...\n", nameof(Case));
    }

    private void ErrorDeletingCases(DatabaseError databaseError)
    {
        const string errorDeletingCases = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorDeletingCases, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Delete(CaseFilter caseFilter)
    {
        try
        {
            DeletingCases();

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            int rowsAffected =

                await dbContext.Cases.Where(caseFilter).ExecuteDeleteAsync();

            if (rowsAffected > 0)

                return UnitResult.Success<DatabaseError>();

            var notFoundError = new NotFoundError
            {
                Message = "No objects found matching the search criteria"
            };

            ErrorDeletingCases(notFoundError);

            return notFoundError;
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to delete objects from database",

                Exception = databaseException
            };

            ErrorDeletingCases(databaseError);

            return databaseError;
        }
    }
}