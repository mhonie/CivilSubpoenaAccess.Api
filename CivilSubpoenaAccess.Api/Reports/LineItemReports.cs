using CSharpFunctionalExtensions;
using CivilSubpoenaAccess.Api.Reports.DataModels.LineItems;

namespace CivilSubpoenaAccess.Api.Reports;


public partial class SubpoenaReports
{
    public async Task<TransactionsBySubpoenaStatus> TransactionsBySubpoenaStatus
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

        (_, bool queryFailure, var transactions, _) =

            await transactionsBySubpoenaInfo.TransactionsBySubpoenaStatus
            (
                subpoenaStatusFilters
            );

        if (!queryFailure)

            return transactions;

        return new TransactionsBySubpoenaStatus
        {
            Error = "Error executing subpoena status counts report"
        };
    }

    public async Task<TransactionsBySubpoenaType> TransactionsBySubpoenaType
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

        (_, bool queryFailure, var transactions, _) =

            await transactionsBySubpoenaInfo.TransactionsBySubpoenaType
            (
                subpoenaTypeFilters
            );

        if (!queryFailure)

            return transactions;

        return new TransactionsBySubpoenaType
        {
            Error = "Error executing subpoena type counts report"
        };
    }

    public async Task<TransactionsByPaymentType> TransactionsByPaymentType
    (
        DateTime approvedStartDate,

        DateTime approvedEndDate
    )
    {
        var paymentTypeFilters = PaymentTypeCountFilters
        (
            approvedStartDate,

            approvedEndDate
        );

        (_, bool queryFailure, var transactions, _) =

            await transactionsByPaymentInfo.TransactionsByPaymentType
            (
                paymentTypeFilters
            );

        if (!queryFailure)

            return transactions;

        return new TransactionsByPaymentType
        {
            Error = "Error executing payment type counts report"
        };
    }

    public async Task<TransactionsByUserType> TransactionsByUserType
    (
        DateTime approvedStartDate,

        DateTime approvedEndDate
    )
    {
        var userTypeFilters = UserTypeFilter
        (
            approvedStartDate,

            approvedEndDate
        );

        (_, bool queryFailure, var transactions, _) =

            await transactionsByUserInfo.TransactionsByUserType
            (
                userTypeFilters
            );

        if (!queryFailure)

            return transactions;

        return new TransactionsByUserType
        {
            Error = "Error executing subpoena type counts report"
        };
    }
}