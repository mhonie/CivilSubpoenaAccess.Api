using CSharpFunctionalExtensions;

using CivilSubpoenaAccess.Api.Reports.Filters;
using CivilSubpoenaAccess.Api.Reports.Enums;
using CivilSubpoenaAccess.Api.Reports.Queries.LineItems;
using CivilSubpoenaAccess.Api.Reports.Queries.Aggregates;
using CivilSubpoenaAccess.Api.Reports.DataModels.Aggregates;
using CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;

namespace CivilSubpoenaAccess.Api.Reports;


public partial class SubpoenaReports
(
    ISubpoenaInfoQueries subpoenaInfoQueries,

    ITransactionsBySubpoenaInfo transactionsBySubpoenaInfo,

    IPaymentInfoQueries paymentInfoQueries,

    ITransactionsByPaymentInfo transactionsByPaymentInfo,

    IUserInfoQueries userInfoQueries,

    ITransactionsByUserInfo transactionsByUserInfo
)
{
    private static SubpoenaStatusFilters SubpoenaStatusFilters
    (
        DateTime approvedStartDate,

        DateTime approvedEndDate,

        DateTime filingStartDate,

        DateTime filingEndDate
    )
    {
        CaseFilter approvedSubpoenaFilter =

            @case =>

                @case.ApproveDate >= approvedStartDate

                && @case.ApproveDate <= approvedEndDate

                && @case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseFilter rejectedSubpoenaFilter =

            @case =>

                @case.ApproveDate >= approvedStartDate

                && @case.ApproveDate <= approvedEndDate

                && @case.ApprovalIndicator == ApprovalIndicator.Rejected;

        CaseFilter pendingPaymentFilter =

            @case =>

                @case.DateFiled >= filingStartDate

                && @case.DateFiled <= filingEndDate

                && @case.PaymentIndicator == PaymentIndicator.WalkIn

                && @case.Status == SubpoenaStatus.Pending;

        CaseFilter pendingApprovalFilter =

            @case =>

                @case.DateFiled >= filingStartDate

                && @case.DateFiled <= filingEndDate

                && @case.PaymentIndicator == PaymentIndicator.InFormaPauperis

                && @case.Status == SubpoenaStatus.Pending;

        var subpoenaStatusFilters = new SubpoenaStatusFilters
        {
            Approved = approvedSubpoenaFilter,

            Rejected = rejectedSubpoenaFilter,

            PendingPayment = pendingPaymentFilter,

            PendingApproval = pendingApprovalFilter,
        };

        return subpoenaStatusFilters;
    }

    public async Task<SubpoenaStatusCounts> SubpoenaStatusCounts
    (
        DateTime approvedStartDate,

        DateTime approvedEndDate,

        DateTime filingStartDate,

        DateTime filingEndDate
    )
    {
        var subpoenaStatusFilters = SubpoenaStatusFilters
        (
            approvedStartDate,

            approvedEndDate,

            filingStartDate,

            filingEndDate
        );

        (_, bool queryFailure, var subpoenaStatusCounts, _) =

            await subpoenaInfoQueries.SubpoenaStatusCounts
            (
                subpoenaStatusFilters
            );

        if (!queryFailure)

            return subpoenaStatusCounts;

        return new SubpoenaStatusCounts
        {
            Error = "Error executing subpoena status counts report"
        };
    }

    private static SubpoenaTypeFilters SubpoenaTypeFilters
    (
        DateTime approvedStartDate,

        DateTime approvedEndDate
    )
    {
        CaseFilter attendTypeFilter =

            @case =>

                @case.ApproveDate >= approvedStartDate

                && @case.ApproveDate <= approvedEndDate

                && @case.ApprovalIndicator == ApprovalIndicator.Approved

                && @case.SubpoenaType == SubpoenaType.Attend;

        CaseFilter produceTypeFilter =

            @case =>

                @case.ApproveDate >= approvedStartDate

                && @case.ApproveDate <= approvedEndDate

                && @case.ApprovalIndicator == ApprovalIndicator.Approved

                && @case.SubpoenaType == SubpoenaType.Produce;

        var subpoenaTypeFilters = new SubpoenaTypeFilters
        {
            Attend = attendTypeFilter,

            Produce = produceTypeFilter
        };

        return subpoenaTypeFilters;
    }

    public async Task<SubpoenaTypeCounts> SubpoenaTypeCounts
    (
        DateTime approvedStartDate,

        DateTime approvedEndDate
    )
    {
        var subpoenaTypeFilters = SubpoenaTypeFilters
        (
            approvedStartDate,

            approvedEndDate
        );

        (_, bool queryFailure, var subpoenaTypeCounts, _) =

            await subpoenaInfoQueries.SubpoenaTypeCounts
            (
                subpoenaTypeFilters
            );

        if (!queryFailure)

            return subpoenaTypeCounts;

        return new SubpoenaTypeCounts
        {
            Error = "Error executing subpoena type counts report"
        };
    }

    private static CaseFilter UserTypeFilter
    (
        DateTime approvedStartDate,

        DateTime approvedEndDate
    )
    {
        CaseFilter caseFilter =

            @case =>

                @case.ApproveDate >= approvedStartDate

                && @case.ApproveDate <= approvedEndDate

                && @case.ApprovalIndicator == ApprovalIndicator.Approved;

        return caseFilter;
    }

    public async Task<UserTypeCounts> UserTypeCounts
    (
        DateTime approvedStartDate,

        DateTime approvedEndDate
    )
    {
        CaseFilter approvedSubpoenaFilter = UserTypeFilter
        (
            approvedStartDate,

            approvedEndDate
        );

        (_, bool queryFailure, var userTypeCounts, _) =

            await userInfoQueries.UserTypeCounts(approvedSubpoenaFilter);

        if (!queryFailure)

            return userTypeCounts;

        return new UserTypeCounts
        {
            Error = "Error executing user type counts report"
        };
    }

    private static PaymentTypeCountFilters PaymentTypeCountFilters
    (
        DateTime approvedStartDate,

        DateTime approvedEndDate
    )
    {
        CaseCartFilter inFormaPauperisFilter =

            @case =>

                @case.Case.ApproveDate >= approvedStartDate

                && @case.Case.ApproveDate <= approvedEndDate

                && @case.Case.PaymentIndicator == PaymentIndicator.InFormaPauperis

                && @case.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseCartFilter citySolicitorFilter =

            @case =>

                @case.Case.ApproveDate >= approvedStartDate

                && @case.Case.ApproveDate <= approvedEndDate

                && @case.Case.PaymentIndicator == PaymentIndicator.CitySolicitor

                && @case.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseCartFilter cityLawFilter =

            @case =>

                @case.Case.ApproveDate >= approvedStartDate

                && @case.Case.ApproveDate <= approvedEndDate

                && @case.Case.PaymentIndicator == PaymentIndicator.CityLaw

                && @case.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseCartFilter walkInsFilter =

            @case =>

                @case.Case.ApproveDate >= approvedStartDate

                && @case.Case.ApproveDate <= approvedEndDate

                && @case.Case.PaymentIndicator == PaymentIndicator.WalkIn

                && @case.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseCartFilter americanExpressFilter =

            caseCart =>

                caseCart.Case.ApproveDate >= approvedStartDate

                && caseCart.Case.ApproveDate <= approvedEndDate

                && caseCart.Cart.CreditCardType == PaymentIndicator.AmericanExpress

                && caseCart.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseCartFilter discoverCardFilter =

            caseCart =>

                caseCart.Case.ApproveDate >= approvedStartDate

                && caseCart.Case.ApproveDate <= approvedEndDate

                && caseCart.Cart.CreditCardType == PaymentIndicator.DiscoverCard

                && caseCart.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseCartFilter mastercardFilter =

            caseCart =>

                caseCart.Case.ApproveDate >= approvedStartDate

                && caseCart.Case.ApproveDate <= approvedEndDate

                && caseCart.Cart.CreditCardType == PaymentIndicator.Mastercard

                && caseCart.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseCartFilter visaCardFilter =

            caseCart =>

                caseCart.Case.ApproveDate >= approvedStartDate

                && caseCart.Case.ApproveDate <= approvedEndDate

                && caseCart.Cart.CreditCardType == PaymentIndicator.VisaCard

                && caseCart.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        var paymentTypeCountFilters = new PaymentTypeCountFilters
        {
            AmericanExpress = americanExpressFilter,

            DiscoverCard = discoverCardFilter,

            Mastercard = mastercardFilter,

            VisaCard = visaCardFilter,

            InFormaPauperis = inFormaPauperisFilter,

            CityLaw = cityLawFilter,

            CitySolicitor = citySolicitorFilter,

            WalkIn = walkInsFilter
        };

        return paymentTypeCountFilters;
    }

    public async Task<PaymentTypeCounts> ApprovedPaymentTypeCounts
    (
        DateTime approvedStartDate,

        DateTime approvedEndDate
    )
    {
        var paymentTypeCountFilters = PaymentTypeCountFilters
        (
            approvedStartDate,

            approvedEndDate
        );

        (_, bool queryFailure, var paymentTypeCounts, _) =

            await paymentInfoQueries.ApprovedPaymentTypeCounts
            (
                paymentTypeCountFilters
            );

        if (!queryFailure)

            return paymentTypeCounts;

        return new PaymentTypeCounts
        {
            Error = "Error executing approved payment type counts report"
        };
    }

    public async Task<PaymentTypeTotals> ApprovedPaymentTypeTotals
    (
        DateTime paidStartDate,

        DateTime paidEndDate
    )
    {
        CartFilter citySolicitorFilter =

            cart =>

                cart.PaidDate >= paidStartDate

                && cart.PaidDate <= paidEndDate

                && cart.CreditCardType == PaymentIndicator.CitySolicitor;

        CartFilter cityLawFilter =

            cart =>

                cart.PaidDate >= paidStartDate

                && cart.PaidDate <= paidEndDate

                && cart.CreditCardType == PaymentIndicator.CityLaw;

        CaseFilter walkInsFilter =

            cart =>

                cart.ApproveDate >= paidStartDate

                && cart.ApproveDate <= paidEndDate

                && cart.ApprovalIndicator == ApprovalIndicator.Approved

                && cart.PaymentIndicator == PaymentIndicator.WalkIn;

        CartFilter americanExpressFilter =

            cart =>

                cart.PaidDate >= paidStartDate

                && cart.PaidDate <= paidEndDate

                && cart.CreditCardType == PaymentIndicator.AmericanExpress;

        CartFilter discoverCardFilter =

            cart =>

                cart.PaidDate >= paidStartDate

                && cart.PaidDate <= paidEndDate

                && cart.CreditCardType == PaymentIndicator.DiscoverCard;

        CartFilter mastercardFilter =

            cart =>

                cart.PaidDate >= paidStartDate

                && cart.PaidDate <= paidEndDate

                && cart.CreditCardType == PaymentIndicator.Mastercard;

        CartFilter visaCardFilter =

            cart =>

                cart.PaidDate >= paidStartDate

                && cart.PaidDate <= paidEndDate

                && cart.CreditCardType == PaymentIndicator.VisaCard;

        var paymentTypeTotalFilters = new PaymentTypeTotalFilters
        {
            AmericanExpress = americanExpressFilter,

            DiscoverCard = discoverCardFilter,

            Mastercard = mastercardFilter,

            VisaCard = visaCardFilter,

            CityLaw = cityLawFilter,

            CitySolicitor = citySolicitorFilter,

            WalkIn = walkInsFilter
        };

        (_, bool queryFailure, var paymentTypeTotals, _) =

            await paymentInfoQueries.ApprovedPaymentTypeTotals(paymentTypeTotalFilters);

        if (!queryFailure)

            return paymentTypeTotals;

        return new PaymentTypeTotals
        {
            Error = "Error executing payment type totals report"
        };
    }

    public async Task<PaymentTypeFees> ApprovedPaymentTypeFees
    (
        DateTime paidStartDate,

        DateTime paidEndDate
    )
    {
        CartFilter americanExpressFilter =

            cart =>

                cart.PaidDate >= paidStartDate

                && cart.PaidDate <= paidEndDate

                && cart.CreditCardType == PaymentIndicator.AmericanExpress;

        CartFilter discoverCardFilter =

            cart =>

                cart.PaidDate >= paidStartDate

                && cart.PaidDate <= paidEndDate

                && cart.CreditCardType == PaymentIndicator.DiscoverCard;

        CartFilter mastercardFilter =

            cart =>

                cart.PaidDate >= paidStartDate

                && cart.PaidDate <= paidEndDate

                && cart.CreditCardType == PaymentIndicator.Mastercard;

        CartFilter visaCardFilter =

            cart =>

                cart.PaidDate >= paidStartDate

                && cart.PaidDate <= paidEndDate

                && cart.CreditCardType == PaymentIndicator.VisaCard;

        var paymentTypeFeeFilters = new PaymentTypeFeeFilters
        {
            AmericanExpress = americanExpressFilter,

            DiscoverCard = discoverCardFilter,

            Mastercard = mastercardFilter,

            VisaCard = visaCardFilter
        };

        (_, bool queryFailure, var paymentTypeFees, _) =

            await paymentInfoQueries.ApprovedPaymentTypeFees(paymentTypeFeeFilters);

        if (!queryFailure)

            return paymentTypeFees;

        return new PaymentTypeFees
        {
            Error = "Error executing payment type fees report"
        };
    }

    private static Dictionary<DateTime, int> CountItemsByMonth
    (
        IQueryable<TransactionInfo>? transactions
    )
    {
        if (transactions is null)

            return [];

        return transactions

            .GroupBy
            (
                transaction => new DateTime
                (
                    transaction.FilingDate!.Value.Year,

                    transaction.FilingDate.Value.Month,

                    1
                )
            )

            .ToDictionary
            (
                group => group.Key,

                group => group.Sum(transaction => transaction.ItemCount)
            );
    }

    private static PaymentTypeCountsByMonth GroupByMonth
    (
        TransactionsByPaymentType transactions,

        DateTime filingStartDate,

        DateTime filingEndDate
    )
    {
        var americanExpress = CountItemsByMonth(transactions.AmericanExpress);

        var discoverCard = CountItemsByMonth(transactions.DiscoverCard);

        var mastercard = CountItemsByMonth(transactions.Mastercard);

        var visaCard = CountItemsByMonth(transactions.VisaCard);

        var inFormaPauperis = CountItemsByMonth(transactions.InFormaPauperis);

        var cityLaw = CountItemsByMonth(transactions.CityLaw);

        var citySolicitor = CountItemsByMonth(transactions.CitySolicitor);

        var walkIn = CountItemsByMonth(transactions.WalkIn);

        var paymentTypeCountsByPeriod = new List<PaymentTypeCountsByPeriod>();

        DateTime currentStartDate = new DateTime
        (
            filingStartDate.Year,

            filingStartDate.Month,

            1
        );

        DateTime finalStartDate = new DateTime
        (
            filingEndDate.Year,

            filingEndDate.Month,

            1
        );

        while (currentStartDate <= finalStartDate)
        {
            DateTime currentEndDate = currentStartDate

                .AddMonths(1)

                .AddTicks(-1);

            paymentTypeCountsByPeriod.Add
            (
                new PaymentTypeCountsByPeriod
                {
                    StartDate = currentStartDate,

                    EndDate = currentEndDate,

                    PaymentTypeCount = new PaymentTypeCounts
                    {
                        InFormaPauperis = inFormaPauperis.GetValueOrDefault(currentStartDate),

                        CitySolicitor = citySolicitor.GetValueOrDefault(currentStartDate),

                        CityLaw = cityLaw.GetValueOrDefault(currentStartDate),

                        WalkIn = walkIn.GetValueOrDefault(currentStartDate),

                        AmericanExpress = americanExpress.GetValueOrDefault(currentStartDate),

                        DiscoverCard = discoverCard.GetValueOrDefault(currentStartDate),

                        Mastercard = mastercard.GetValueOrDefault(currentStartDate),

                        VisaCard = visaCard.GetValueOrDefault(currentStartDate)
                    }
                }
            );

            currentStartDate = currentStartDate.AddMonths(1);
        }

        return new PaymentTypeCountsByMonth
        {
            PaymentTypeCountsByPeriod = paymentTypeCountsByPeriod.ToArray()
        };
    }

    public async Task<PaymentTypeCountsByMonth> MonthlyPaymentTypeCounts
    (
        DateTime filingStartDate,

        DateTime filingEndDate
    )
    {
        CaseCartFilter inFormaPauperisFilter =

            @case =>

                @case.Case.DateFiled >= filingStartDate

                && @case.Case.DateFiled <= filingEndDate

                && @case.Case.PaymentIndicator == PaymentIndicator.InFormaPauperis;

        CaseCartFilter citySolicitorFilter =

            @case =>

                @case.Case.DateFiled >= filingStartDate

                && @case.Case.DateFiled <= filingEndDate

                && @case.Case.PaymentIndicator == PaymentIndicator.CitySolicitor;

        CaseCartFilter cityLawFilter =

            @case =>

                @case.Case.DateFiled >= filingStartDate

                && @case.Case.DateFiled <= filingEndDate

                && @case.Case.PaymentIndicator == PaymentIndicator.CityLaw;

        CaseCartFilter walkInsFilter =

            @case =>

                @case.Case.DateFiled >= filingStartDate

                && @case.Case.DateFiled <= filingEndDate

                && @case.Case.PaymentIndicator == PaymentIndicator.WalkIn;

        CaseCartFilter americanExpressFilter =

            caseCart =>

                caseCart.Case.DateFiled >= filingStartDate

                && caseCart.Case.DateFiled <= filingEndDate

                && caseCart.Cart.CreditCardType == PaymentIndicator.AmericanExpress

                && caseCart.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseCartFilter discoverCardFilter =

            caseCart =>

                caseCart.Case.DateFiled >= filingStartDate

                && caseCart.Case.DateFiled <= filingEndDate

                && caseCart.Cart.CreditCardType == PaymentIndicator.DiscoverCard

                && caseCart.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseCartFilter mastercardFilter =

            caseCart =>

                caseCart.Case.DateFiled >= filingStartDate

                && caseCart.Case.DateFiled <= filingEndDate

                && caseCart.Cart.CreditCardType == PaymentIndicator.Mastercard

                && caseCart.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        CaseCartFilter visaCardFilter =

            caseCart =>

                caseCart.Case.DateFiled >= filingStartDate

                && caseCart.Case.DateFiled <= filingEndDate

                && caseCart.Cart.CreditCardType == PaymentIndicator.VisaCard

                && caseCart.Case.ApprovalIndicator == ApprovalIndicator.Approved;

        var paymentTypeCountFilters = new PaymentTypeCountFilters
        {
            AmericanExpress = americanExpressFilter,

            DiscoverCard = discoverCardFilter,

            Mastercard = mastercardFilter,

            VisaCard = visaCardFilter,

            InFormaPauperis = inFormaPauperisFilter,

            CityLaw = cityLawFilter,

            CitySolicitor = citySolicitorFilter,

            WalkIn = walkInsFilter
        };

        (_, bool queryFailure, var transactions, _) =

            await transactionsByPaymentInfo.TransactionsByPaymentType
            (
                paymentTypeCountFilters
            );

        if (queryFailure)

            return new PaymentTypeCountsByMonth
            {
                Error = "Error executing monthly payment type counts query"
            };

        return GroupByMonth
        (
            transactions,

            filingStartDate,

            filingEndDate
        );
    }
}