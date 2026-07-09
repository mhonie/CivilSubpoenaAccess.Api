using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

using CivilSubpoenaAccess.Api.Database;
using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Reports.Filters;
using CivilSubpoenaAccess.Api.Serialization;
using CivilSubpoenaAccess.Api.Reports.DataModels;
using CivilSubpoenaAccess.Api.Reports.DataModels.Aggregates;
using System.Linq;

namespace CivilSubpoenaAccess.Api.Reports.Queries.Aggregates;


public interface IPaymentInfoQueries
{
    Task<Result<PaymentTypeTotals, DatabaseError>> ApprovedPaymentTypeTotals
    (
        PaymentTypeTotalFilters paymentTypeTotalFilters
    );

    Task<Result<PaymentTypeFees, DatabaseError>> ApprovedPaymentTypeFees
    (
        PaymentTypeFeeFilters paymentTypeFeeFilters
    );

    Task<Result<PaymentTypeCounts, DatabaseError>> ApprovedPaymentTypeCounts
    (
        PaymentTypeCountFilters paymentTypeCountFilters
    );

    Task<Result<PaymentTypeCounts, DatabaseError>> PaymentTypeCounts
    (
        PaymentTypeCountFilters paymentTypeCountFilters
    );
}

public class PaymentInfoQueries
(
    IDbContextFactory<EFilingDatabaseContext> efilingDbContextFactory,

    ILogger serilogger
)
    : IndentingFormatter, IPaymentInfoQueries
{
    private readonly decimal? WalkInPrice = 7.7m;

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

    private void ExecutingPaymentTypeTotals()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(ApprovedPaymentTypeTotals));
    }

    public async Task<Result<PaymentTypeTotals, DatabaseError>> ApprovedPaymentTypeTotals
    (
        PaymentTypeTotalFilters paymentTypeTotalsFilters
    )
    {
        try
        {
            ExecutingPaymentTypeTotals();

            await using var efilingDbContext =

                await efilingDbContextFactory.CreateDbContextAsync();

            decimal? citySolicitorTotal = await efilingDbContext.Carts

                .AsNoTracking()

                .Where(paymentTypeTotalsFilters.CitySolicitor)

                .SumAsync(cart => cart.TotalPrice);

            decimal? cityLawTotal = await efilingDbContext.Carts

                .AsNoTracking()

                .Where(paymentTypeTotalsFilters.CityLaw)

                .SumAsync(cart => cart.TotalPrice);

            decimal? walkInTotal = await efilingDbContext.Cases

                .AsNoTracking()

                .CountAsync(paymentTypeTotalsFilters.WalkIn) * WalkInPrice;

            decimal? americanExpressTotal = await efilingDbContext.Carts

                .AsNoTracking()

                .Where(paymentTypeTotalsFilters.AmericanExpress)

                .SumAsync(cart => cart.TotalPrice);

            decimal? mastercardTotal = await efilingDbContext.Carts

                .AsNoTracking()

                .Where(paymentTypeTotalsFilters.Mastercard)

                .SumAsync(cart => cart.TotalPrice);

            decimal? discoverCardTotal = await efilingDbContext.Carts

                .AsNoTracking()

                .Where(paymentTypeTotalsFilters.DiscoverCard)

                .SumAsync(cart => cart.TotalPrice);

            decimal? visaCardTotal = await efilingDbContext.Carts

                .AsNoTracking()

                .Where(paymentTypeTotalsFilters.VisaCard)

                .SumAsync(cart => cart.TotalPrice);

            return new PaymentTypeTotals
            {
                InFormaPauperis = 0,

                CityLaw = cityLawTotal,

                CitySolicitor = citySolicitorTotal,

                WalkIn = walkInTotal,

                AmericanExpress = americanExpressTotal,

                DiscoverCard = discoverCardTotal,

                Mastercard = mastercardTotal,

                VisaCard = visaCardTotal
            };
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Error executing {nameof(ApprovedPaymentTypeTotals)} query",

                Exception = databaseException
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
    }

    private void ExecutingPaymentTypeFees()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(ApprovedPaymentTypeFees));
    }

    private readonly decimal? ConvenienceFee = 5.0m;

    public async Task<Result<PaymentTypeFees, DatabaseError>> ApprovedPaymentTypeFees
    (
        PaymentTypeFeeFilters paymentTypeFeeFilters
    )
    {
        try
        {
            ExecutingPaymentTypeFees();

            await using var efilingDbContext =

                await efilingDbContextFactory.CreateDbContextAsync();

            int americanExpressCount = await efilingDbContext.Carts

                .AsNoTracking()

                .CountAsync(paymentTypeFeeFilters.AmericanExpress);

            int discoverCardCount = await efilingDbContext.Carts

                .AsNoTracking()

                .CountAsync(paymentTypeFeeFilters.DiscoverCard);

            int mastercardCount = await efilingDbContext.Carts

                .AsNoTracking()

                .CountAsync(paymentTypeFeeFilters.Mastercard);

            int visaCardCount = await efilingDbContext.Carts

                .AsNoTracking()

                .CountAsync(paymentTypeFeeFilters.VisaCard);

            return new PaymentTypeFees
            {
                InFormaPauperis = 0,

                CityLaw = 0,

                CitySolicitor = 0,

                WalkIn = 0,

                AmericanExpress = americanExpressCount * ConvenienceFee,

                DiscoverCard = discoverCardCount * ConvenienceFee,

                Mastercard = mastercardCount * ConvenienceFee,

                VisaCard = visaCardCount * ConvenienceFee
            };
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Error executing {nameof(ApprovedPaymentTypeTotals)} query",

                Exception = databaseException
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
    }

    private void ExecutingApprovedPaymentTypeCounts()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(ApprovedPaymentTypeCounts));
    }

    public async Task<Result<PaymentTypeCounts, DatabaseError>> ApprovedPaymentTypeCounts
    (
         PaymentTypeCountFilters paymentTypeCountFilters
    )
    {
        try
        {
            ExecutingApprovedPaymentTypeCounts();

            await using var efilingDbContext =

                await efilingDbContextFactory.CreateDbContextAsync();

            IQueryable<CaseCart> caseCarts = efilingDbContext.Cases

                .AsNoTracking()

                .Join
                (
                    efilingDbContext.Carts.AsNoTracking(),

                    @case => @case.TransactionId,

                    cart => cart.TransactionId,

                    (@case, cart) => new CaseCart
                    {
                        Case = @case,

                        Cart = cart
                    }
                );

            int inFormaPauperisCount = await caseCarts

                .Where(paymentTypeCountFilters.InFormaPauperis)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int citySolicitorCount = await caseCarts

                .Where(paymentTypeCountFilters.CitySolicitor)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int cityLawCount = await caseCarts

                .Where(paymentTypeCountFilters.CityLaw)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int walkInCount = await caseCarts

                .Where(paymentTypeCountFilters.WalkIn)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int americanExpressCount = await caseCarts

                .Where(paymentTypeCountFilters.AmericanExpress)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int discoverCardCount = await caseCarts

                .Where(paymentTypeCountFilters.DiscoverCard)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int mastercardCount = await caseCarts

                .Where(paymentTypeCountFilters.Mastercard)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int visaCardCount = await caseCarts

                .Where(paymentTypeCountFilters.VisaCard)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            return new PaymentTypeCounts
            {
                InFormaPauperis = inFormaPauperisCount,

                CitySolicitor = citySolicitorCount,

                CityLaw = cityLawCount,

                WalkIn = walkInCount,

                AmericanExpress = americanExpressCount,

                DiscoverCard = discoverCardCount,

                Mastercard = mastercardCount,

                VisaCard = visaCardCount
            };
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Error executing {nameof(ApprovedPaymentTypeCounts)} query",

                Exception = databaseException
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
    }

    private void ExecutingPaymentTypeCounts()
    {
        serilogger.Debug("Executing {0} query...\n", nameof(PaymentTypeCounts));
    }

    public async Task<Result<PaymentTypeCounts, DatabaseError>> PaymentTypeCounts
    (
        PaymentTypeCountFilters paymentTypeCountFilters
    )
    {
        try
        {
            ExecutingPaymentTypeCounts();

            await using var efilingDbContext =

                await efilingDbContextFactory.CreateDbContextAsync();

            IQueryable<CaseCart> caseCarts = efilingDbContext.Cases

                .AsNoTracking()

                .Join
                (
                    efilingDbContext.Carts.AsNoTracking(),

                    @case => @case.TransactionId,

                    cart => cart.TransactionId,

                    (@case, cart) => new CaseCart
                    {
                        Case = @case,

                        Cart = cart
                    }
                );

            int inFormaPauperisCount = await caseCarts

                .Where(paymentTypeCountFilters.InFormaPauperis)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int citySolicitorCount = await caseCarts

                .Where(paymentTypeCountFilters.CitySolicitor)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int cityLawCount = await caseCarts

                .Where(paymentTypeCountFilters.CityLaw)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int walkInCount = await caseCarts

                .Where(paymentTypeCountFilters.WalkIn)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int americanExpressCount = await caseCarts

                .Where(paymentTypeCountFilters.AmericanExpress)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int discoverCardCount = await caseCarts

                .Where(paymentTypeCountFilters.DiscoverCard)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int mastercardCount = await caseCarts

                .Where(paymentTypeCountFilters.Mastercard)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            int visaCardCount = await caseCarts

                .Where(paymentTypeCountFilters.VisaCard)

                .SumAsync(x => x.Cart.NumberOfItems) ?? 0;

            return new PaymentTypeCounts
            {
                InFormaPauperis = inFormaPauperisCount,

                CitySolicitor = citySolicitorCount,

                CityLaw = cityLawCount,

                WalkIn = walkInCount,

                AmericanExpress = americanExpressCount,

                DiscoverCard = discoverCardCount,

                Mastercard = mastercardCount,

                VisaCard = visaCardCount
            };
        }
        catch (Exception databaseException)
        {
            var databaseError = new DatabaseError
            {
                Message = $"Error executing {nameof(PaymentTypeCounts)} query",

                Exception = databaseException
            };

            ErrorExecutingQuery(databaseError);

            return databaseError;
        }
    }
}