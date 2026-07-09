using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;

using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Serialization;
using CivilSubpoenaAccess.Api.Database.Entities;

namespace CivilSubpoenaAccess.Api.Database.Repositories;


public interface ICartRepository
{
    Task<UnitResult<DatabaseError>> Create(Cart cart);

    Task<UnitResult<DatabaseError>> Create(Cart[] carts);

    Task<Result<Cart, DatabaseError>> Read(string transactionId);

    Task<Result<Cart[], DatabaseError>> Read(CartFilter cartFilter);

    Task<UnitResult<DatabaseError>> Update(Cart oldCart, Cart newCart);

    Task<UnitResult<DatabaseError>> Update(Cart[] oldCarts, Cart[] newCarts);

    Task<UnitResult<DatabaseError>> Delete(string transactionId);

    Task<UnitResult<DatabaseError>> Delete(CartFilter cartFilter);
}

public class CartRepository(IDbContextFactory<EFilingDatabaseContext> dbContextFactory, ILogger serilogger)

    : IndentingFormatter, ICartRepository
{
    private void CreatingCart(string transactionId)
    {
        serilogger.Debug
        (
            "Creating {0} with {1}={2}...\n", 
            
            nameof(Cart), 
            
            nameof(Cart.TransactionId), 
            
            transactionId
        );
    }

    private void ErrorCreatingCart(DatabaseError databaseError)
    {
        const string errorCreatingCart = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorCreatingCart, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Create(Cart cart)
    {
        try
        {
            CreatingCart(cart.TransactionId);

            await using var dbContext = 
                
                await dbContextFactory.CreateDbContextAsync();

            await dbContext.Carts.AddAsync(cart);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to create cart object",

                Cart = cart,

                Exception = databaseException
            };

            ErrorCreatingCart(databaseError);

            return databaseError;
        }
    }

    private void CreatingCarts()
    {
        serilogger.Debug("Creating {0} objects...\n", nameof(Cart));
    }

    private void ErrorCreatingCarts(DatabaseError databaseError)
    {
        const string errorCreatingCarts = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorCreatingCarts, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Create(Cart[] carts)
    {
        try
        {
            CreatingCarts();

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            await dbContext.Carts.AddRangeAsync(carts);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to create cart objects",

                Carts = carts,

                Exception = databaseException
            };

            ErrorCreatingCarts(databaseError);

            return databaseError;
        }
    }

    private void ReadingCart(string transactionId)
    {
        serilogger.Debug
        (
            "Reading {0} with {1}={2}...\n", 
            
            nameof(Cart), 
            
            nameof(Cart.TransactionId), 
            
            transactionId
        );
    }

    private void ErrorReadingCart(DatabaseError databaseError)
    {
        const string errorReadingCart = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorReadingCart, databaseErrorJson);
    }

    public async Task<Result<Cart, DatabaseError>> Read(string transactionId)
    {
        try
        {
            ReadingCart(transactionId);

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var cart =

                await dbContext.Carts.FirstOrDefaultAsync
                (
                    entity => entity.TransactionId == transactionId
                );

            if (cart != null)

                return cart;

            var notFoundError = new NotFoundError
            {
                Message = $"Object not found: {nameof(Cart)} " +
                          
                          $"with {nameof(Cart.TransactionId)}={transactionId}"
            };

            ErrorReadingCart(notFoundError);

            return notFoundError;
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Failed to read {nameof(Cart)} " +
                          
                          $"with {nameof(Cart.TransactionId)}={transactionId}",

                Exception = databaseException
            };

            ErrorReadingCart(databaseError);

            return databaseError;
        }
    }

    private void ReadingCarts()
    {
        serilogger.Debug("Reading {0} table...\n", nameof(Cart));
    }

    private void ErrorReadingCarts(DatabaseError databaseError)
    {
        const string errorReadingCarts = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorReadingCarts, databaseErrorJson);
    }

    public async Task<Result<Cart[], DatabaseError>> Read(CartFilter cartFilter)
    {
        try
        {
            ReadingCarts();

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var carts = await dbContext.Carts.Where(cartFilter).ToArrayAsync();

            return Result.Success<Cart[], DatabaseError>(carts);
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Failed to read {nameof(Cart)} objects from database",

                Exception = databaseException
            };

            ErrorReadingCarts(databaseError);

            return databaseError;
        }
    }

    private void UpdatingCart(string transactionId)
    {
        serilogger.Debug
        (
            "Updating {0} with {1}={2}...\n", 
            
            nameof(Cart), 
            
            nameof(Cart.TransactionId), 
            
            transactionId
        );
    }

    private void ErrorUpdatingCart(DatabaseError databaseError)
    {
        const string errorUpdatingCart = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorUpdatingCart, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Update(Cart oldCart, Cart newCart)
    {
        try
        {
            string transactionId = oldCart.TransactionId;

            UpdatingCart(transactionId);

            if (oldCart.TransactionId != newCart.TransactionId)
            {
                var databaseError = new DatabaseError
                {
                    Message = $"{nameof(Cart.TransactionId)} cannot be changed",

                    Carts = [oldCart, newCart]
                };

                ErrorUpdatingCart(databaseError);

                return databaseError;
            }

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var existingCart =

                await dbContext.Carts.FirstOrDefaultAsync
                (
                    entity => entity.TransactionId == transactionId
                );

            if (existingCart == null)
            {
                var notFoundError = new NotFoundError
                {
                    Message = $"Object not found: {nameof(Cart)} " +

                              $"with {nameof(Cart.TransactionId)}={transactionId}"
                };

                ErrorUpdatingCart(notFoundError);

                return notFoundError;
            }

            dbContext.Entry(existingCart).CurrentValues.SetValues(newCart);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Failed to update {nameof(Cart)} object in database",

                Carts = [oldCart, newCart],

                Exception = databaseException
            };

            ErrorUpdatingCart(databaseError);

            return databaseError;
        }
    }

    private void UpdatingCarts()
    {
        serilogger.Debug("Updating {0} objects...\n", nameof(Cart));
    }

    private void ErrorUpdatingCarts(DatabaseError databaseError)
    {
        const string errorUpdatingCarts = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorUpdatingCarts, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Update(Cart[] oldCarts, Cart[] newCarts)
    {
        try
        {
            UpdatingCarts();

            if (oldCarts.Length != newCarts.Length)
            {
                var allCarts = oldCarts.Concat(newCarts).ToArray();

                var databaseError = new DatabaseError
                {
                    Message = $"Inconsistent {nameof(Cart)} updates",

                    Carts = allCarts
                };

                ErrorUpdatingCarts(databaseError);

                return databaseError;
            }

            if (oldCarts.Where((cart, index) => cart.TransactionId != newCarts[index].TransactionId).Any())
            {
                var allCarts = oldCarts.Concat(newCarts).ToArray();

                var databaseError = new DatabaseError
                {
                    Message = $"{nameof(Cart.TransactionId)} cannot be changed",

                    Carts = allCarts
                };

                ErrorUpdatingCarts(databaseError);

                return databaseError;
            }

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.Carts.UpdateRange(newCarts);

            await dbContext.SaveChangesAsync();

            return UnitResult.Success<DatabaseError>();
        }
        catch (Exception databaseException)
        {
            var allCarts = oldCarts.Concat(newCarts).ToArray();

            var databaseError = new DatabaseError
            {
                Message = $"Failed to update {nameof(Cart)} objects in database",

                Carts = allCarts,

                Exception = databaseException
            };

            ErrorUpdatingCarts(databaseError);

            return databaseError;
        }
    }

    private void DeletingCart(string transactionId)
    {
        serilogger.Debug
        (
            "Deleting {0} with {1}={2}...\n", 
            
            nameof(Cart), 
            
            nameof(Cart.TransactionId), 
            
            transactionId
        );
    }

    private void ErrorDeletingCart(DatabaseError databaseError)
    {
        const string errorDeletingCart = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorDeletingCart, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Delete(string transactionId)
    {
        try
        {
            DeletingCart(transactionId);

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            int rowsAffected =

                await dbContext.Carts
                    
                    .Where(entity => entity.TransactionId == transactionId)

                    .ExecuteDeleteAsync();

            if (rowsAffected > 0)

                return UnitResult.Success<DatabaseError>();

            var notFoundError = new NotFoundError
            {
                Message = $"Object not found: {nameof(Cart)} " +

                          $"with {nameof(Cart.TransactionId)}={transactionId}"
            };

            ErrorDeletingCart(notFoundError);

            return notFoundError;
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to delete object from database",

                Exception = databaseException
            };

            ErrorDeletingCart(databaseError);

            return databaseError;
        }
    }

    private void DeletingCarts()
    {
        serilogger.Debug("Deleting {0} objects...\n", nameof(Cart));
    }

    private void ErrorDeletingCarts(DatabaseError databaseError)
    {
        const string errorDeletingCarts = "{0}";

        string databaseErrorJson = ToJson(databaseError);

        serilogger.Error(databaseError.Exception, errorDeletingCarts, databaseErrorJson);
    }

    public async Task<UnitResult<DatabaseError>> Delete(CartFilter cartFilter)
    {
        try
        {
            DeletingCarts();

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            int rowsAffected =

                await dbContext.Carts.Where(cartFilter).ExecuteDeleteAsync();

            if (rowsAffected > 0)

                return UnitResult.Success<DatabaseError>();

            var notFoundError = new NotFoundError
            {
                Message = "No objects found matching the search criteria"
            };

            ErrorDeletingCarts(notFoundError);

            return notFoundError;
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = "Failed to delete objects from database",

                Exception = databaseException
            };

            ErrorDeletingCarts(databaseError);

            return databaseError;
        }
    }
}