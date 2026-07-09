using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Serialization;
using CivilSubpoenaAccess.Api.Database.Entities;

namespace CivilSubpoenaAccess.Api.Database.Repositories;


public interface IUserRepository
{
    Task<UnitResult<DatabaseError>> Create(User user);

    Task<UnitResult<DatabaseError>> Create(User[] users);

    Task<Result<User, DatabaseError>> Read(string userName);

    Task<Result<User[], DatabaseError>> Read(UserFilter userFilter);

    Task<UnitResult<DatabaseError>> Update(User oldUser, User newUser);

    Task<UnitResult<DatabaseError>> Update(User[] oldUsers, User[] newUsers);

    Task<UnitResult<DatabaseError>> Delete(string userName);

    Task<UnitResult<DatabaseError>> Delete(UserFilter userFilter);
}

public class UserRepository(IDbContextFactory<EFilingDatabaseContext> dbContextFactory, ILogger serilogger)

    : IndentingFormatter, IUserRepository
{
    private void CreatingUser(string userName)
    {
        serilogger.Debug
        (
            "Creating {0} with {1}={2}...\n",

            nameof(User),

            nameof(User.UserName),

            userName
        );
    }

    private void ErrorCreatingUser(DatabaseError databaseError)
    {
        const string errorCreatingUser = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorCreatingUser, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Create(User user)
    {
        try
        {
            CreatingUser(user.UserName);

            await using var dbContext =

                await dbContextFactory.CreateDbContextAsync();

            await dbContext.Users.AddAsync(user);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to create user object",

                User = user,

                Exception = databaseException
            };

            ErrorCreatingUser(databaseError);

            return databaseError;
        }
    }

    private void CreatingUsers()
    {
        serilogger.Debug("Creating {0} objects...\n", nameof(User));
    }

    private void ErrorCreatingUsers(DatabaseError databaseError)
    {
        const string errorCreatingUsers = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorCreatingUsers, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Create(User[] users)
    {
        try
        {
            CreatingUsers();

            await using var dbContext =

                await dbContextFactory.CreateDbContextAsync();

            await dbContext.Users.AddRangeAsync(users);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to create user objects",

                Users = users,

                Exception = databaseException
            };

            ErrorCreatingUsers(databaseError);

            return databaseError;
        }
    }

    private void ReadingUser(string userName)
    {
        serilogger.Debug
        (
            "Reading {0} with {1}={2}...\n",

            nameof(User),

            nameof(User.UserName),

            userName
        );
    }

    private void ErrorReadingUser(DatabaseError databaseError)
    {
        const string errorReadingUser = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorReadingUser, databaseErrorJson);
    }

    public async Task<Result<User, DatabaseError>> Read(string userName)
    {
        try
        {
            ReadingUser(userName);

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var user =

                await dbContext.Users.FirstOrDefaultAsync
                (
                    entity => entity.UserName == userName
                );

            if (user != null)

                return user;

            var notFoundError = new NotFoundError
            {
                Message = $"Object not found: {nameof(User)} " +

                          $"with {nameof(User.UserName)}={userName}"
            };

            ErrorReadingUser(notFoundError);

            return notFoundError;
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Failed to read {nameof(User)} " +

                          $"with {nameof(User.UserName)}={userName}",

                Exception = databaseException
            };

            ErrorReadingUser(databaseError);

            return databaseError;
        }
    }

    private void ReadingUsers()
    {
        serilogger.Debug("Reading {0} table...\n", nameof(User));
    }

    private void ErrorReadingUsers(DatabaseError databaseError)
    {
        const string errorReadingUsers = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorReadingUsers, databaseErrorJson);
    }

    public async Task<Result<User[], DatabaseError>> Read(UserFilter userFilter)
    {
        try
        {
            ReadingUsers();

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var users = await dbContext.Users.Where(userFilter).ToArrayAsync();

            return Result.Success<User[], DatabaseError>(users);
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Failed to read {nameof(User)} objects from database",

                Exception = databaseException
            };

            ErrorReadingUsers(databaseError);

            return databaseError;
        }
    }

    private void UpdatingUser(string userName)
    {
        serilogger.Debug
        (
            "Updating {0} with {1}={2}...\n",

            nameof(User),

            nameof(User.UserName),

            userName
        );
    }

    private void ErrorUpdatingUser(DatabaseError databaseError)
    {
        const string errorUpdatingUser = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorUpdatingUser, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Update(User oldUser, User newUser)
    {
        try
        {
            string userName = oldUser.UserName;

            UpdatingUser(userName);

            if (oldUser.UserName != newUser.UserName)
            {
                var databaseError = new DatabaseError
                {
                    Message = $"{nameof(User.UserName)} cannot be changed",

                    Users = [oldUser, newUser]
                };

                ErrorUpdatingUser(databaseError);

                return databaseError;
            }

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var existingUser =

                await dbContext.Users.FirstOrDefaultAsync
                (
                    entity => entity.UserName == userName
                );

            if (existingUser == null)
            {
                var notFoundError = new NotFoundError
                {
                    Message = $"Object not found: {nameof(User)} " +

                              $"with {nameof(User.UserName)}={userName}"
                };

                ErrorUpdatingUser(notFoundError);

                return notFoundError;
            }

            dbContext.Entry(existingUser).CurrentValues.SetValues(newUser);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Failed to update {nameof(User)} object in database",

                Users = [oldUser, newUser],

                Exception = databaseException
            };

            ErrorUpdatingUser(databaseError);

            return databaseError;
        }
    }

    private void UpdatingUsers()
    {
        serilogger.Debug("Updating {0} objects...\n", nameof(User));
    }

    private void ErrorUpdatingUsers(DatabaseError databaseError)
    {
        const string errorUpdatingUsers = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorUpdatingUsers, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Update(User[] oldUsers, User[] newUsers)
    {
        try
        {
            UpdatingUsers();

            if (oldUsers.Length != newUsers.Length)
            {
                var allUsers = oldUsers.Concat(newUsers).ToArray();

                var databaseError = new DatabaseError
                {
                    Message = $"Inconsistent {nameof(User)} updates",

                    Users = allUsers
                };

                ErrorUpdatingUsers(databaseError);

                return databaseError;
            }

            if
            (
                oldUsers.Where((user, index) => user.UserName != newUsers[index].UserName).Any()
            )
            {
                var allUsers = oldUsers.Concat(newUsers).ToArray();

                var databaseError = new DatabaseError
                {
                    Message = $"{nameof(User.UserName)} cannot be changed",

                    Users = allUsers
                };

                ErrorUpdatingUsers(databaseError);

                return databaseError;
            }

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.Users.UpdateRange(newUsers);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var allUsers = oldUsers.Concat(newUsers).ToArray();

            var databaseError = new DatabaseError
            {
                Message = $"Failed to update {nameof(User)} objects in database",

                Users = allUsers,

                Exception = databaseException
            };

            ErrorUpdatingUsers(databaseError);

            return databaseError;
        }
    }

    private void DeletingUser(string userName)
    {
        serilogger.Debug
        (
            "Deleting {0} with {1}={2}...\n",

            nameof(User),

            nameof(User.UserName),

            userName
        );
    }

    private void ErrorDeletingUser(DatabaseError databaseError)
    {
        const string errorDeletingUser = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorDeletingUser, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Delete(string userName)
    {
        try
        {
            DeletingUser(userName);

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            int rowsAffected =

                await dbContext.Users

                    .Where(entity => entity.UserName == userName)

                    .ExecuteDeleteAsync();

            if (rowsAffected > 0)

                return UnitResult.Success<DatabaseError>();

            var notFoundError = new NotFoundError
            {
                Message = $"Object not found: {nameof(User)} " +

                          $"with {nameof(User.UserName)}={userName}"
            };

            ErrorDeletingUser(notFoundError);

            return notFoundError;
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to delete object from database",

                Exception = databaseException
            };

            ErrorDeletingUser(databaseError);

            return databaseError;
        }
    }

    private void DeletingUsers()
    {
        serilogger.Debug("Deleting {0} objects...\n", nameof(User));
    }

    private void ErrorDeletingUsers(DatabaseError databaseError)
    {
        const string errorDeletingUsers = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorDeletingUsers, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Delete(UserFilter userFilter)
    {
        try
        {
            DeletingUsers();

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            int rowsAffected =

                await dbContext.Users.Where(userFilter).ExecuteDeleteAsync();

            if (rowsAffected > 0)

                return UnitResult.Success<DatabaseError>();

            var notFoundError = new NotFoundError
            {
                Message = "No objects found matching the search criteria"
            };

            ErrorDeletingUsers(notFoundError);

            return notFoundError;
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to delete objects from database",

                Exception = databaseException
            };

            ErrorDeletingUsers(databaseError);

            return databaseError;
        }
    }
}