import { useCallback } from "react";

import {
  Alert,
  Box,
  Card,
  CardContent,
  CircularProgress,
  Divider,
  Grid,
  Typography
} from "@mui/material";

import { useReportsContext } from "../../utils/contextUtils";
import { toUtcEndDate, toUtcStartDate } from "../../utils/dateUtils";

import {
  getApprovedPaymentTypeCounts,
  getCreditCardPaymentFees,
  getCreditCardPaymentTotals,
  type FeesByCreditCardType,
  type PaymentTypeCounts,
  type PaymentsByCreditCardType
} from "../../api/subpoenaCounts";

import useSubpoenaReports from "../../hooks/useSubpoenaReports";

import {
  PaymentTypeDataRow,
  type PaymentTypeDataRowModel
} from "../../components/tables/PaymentTypeDataRows";

import {
  formatCount,
  formatCurrency,
  totalCreditCardFees,
  totalCreditCardPayments,
  totalCreditCardTransactions
} from "../../utils/mathUtils";

export default function PaymentsByTypeCard() {

  const {
    approvedStartDate,
    approvedEndDate,
    dateError
  } = useReportsContext();

  const loadPayments = useCallback(
    async () => {

      if (dateError) {
        return {
          americanExpress: 0,
          discoverCard: 0,
          mastercard: 0,
          visaCard: 0,
          inFormaPauperis: 0,
          citySolicitor: 0,
          cityLaw: 0,
          walkIn: 0,
        };
      }

      return getCreditCardPaymentTotals(
        toUtcStartDate(approvedStartDate),
        toUtcEndDate(approvedEndDate)
      );
    },
    [
      dateError,
      approvedStartDate,
      approvedEndDate
    ]
  );

  const loadFees = useCallback(
    async () => {

      if (dateError) {
        return {
          americanExpress: 0,
          discoverCard: 0,
          mastercard: 0,
          visaCard: 0,
          inFormaPauperis: 0,
          citySolicitor: 0,
          cityLaw: 0,
          walkIn: 0,
        };
      }

      return getCreditCardPaymentFees(
        toUtcStartDate(approvedStartDate),
        toUtcEndDate(approvedEndDate)
      );
    },
    [
      dateError,
      approvedStartDate,
      approvedEndDate
    ]
  );

  const loadCounts = useCallback(
    async () => {

      if (dateError) {
        return {
          americanExpress: 0,
          discoverCard: 0,
          mastercard: 0,
          visaCard: 0,
          inFormaPauperis: 0,
          citySolicitor: 0,
          cityLaw: 0,
          walkIn: 0,
        };
      }

      return getApprovedPaymentTypeCounts(
        toUtcStartDate(approvedStartDate),
        toUtcEndDate(approvedEndDate)
      );
    },
    [
      dateError,
      approvedStartDate,
      approvedEndDate
    ]
  );

  const {
    data: paymentsByCreditCardType,
    loading: paymentsLoading,
    error: paymentsError
  } = useSubpoenaReports<PaymentsByCreditCardType>(
    loadPayments,
    "Unable to load credit card payment information."
  );

  const {
    data: feesByCreditCardType,
    loading: feesLoading,
    error: feesError
  } = useSubpoenaReports<FeesByCreditCardType>(
    loadFees,
    "Unable to load credit card fee information."
  );

  const {
    data: approvedPaymentTypeCounts,
    loading: countsLoading,
    error: countsError
  } = useSubpoenaReports<PaymentTypeCounts>(
    loadCounts,
    "Unable to load payment type counts."
  );

  const loading = paymentsLoading || feesLoading || countsLoading;

  const error = paymentsError || feesError || countsError;

  const dataRows: PaymentTypeDataRowModel[] =
    paymentsByCreditCardType && feesByCreditCardType && approvedPaymentTypeCounts
    ? [
        {
          label: "IFP",
          total: 0,
          fees: 0,
          count: approvedPaymentTypeCounts.inFormaPauperis,
          report: "paymentType",
          category: "inFormaPauperis"
        },
        {
          label: "Walk-Ins",
          total: 0,
          fees: 0,
          count: approvedPaymentTypeCounts.walkIn,
          report: "paymentType",
          category: "walkIn"
        },
        {
          label: "City Law",
          total: 0,
          fees: 0,
          count: approvedPaymentTypeCounts.cityLaw,
          report: "paymentType",
          category: "cityLaw"
        },
        {
          label: "City Solicitor",
          total: 0,
          fees: 0,
          count: approvedPaymentTypeCounts.citySolicitor,
          report: "paymentType",
          category: "citySolicitor"
        },
        {
          label: "AMEX",
          total: paymentsByCreditCardType.americanExpress,
          fees: feesByCreditCardType.americanExpress,
          count: approvedPaymentTypeCounts.americanExpress,
          report: "paymentType",
          category: "americanExpress"
        },
        {
          label: "Discover",
          total: paymentsByCreditCardType.discoverCard,
          fees: feesByCreditCardType.discoverCard,
          count: approvedPaymentTypeCounts.discoverCard,
          report: "paymentType",
          category: "discoverCard"
        },
        {
          label: "Mastercard",
          total: paymentsByCreditCardType.mastercard,
          fees: feesByCreditCardType.mastercard,
          count: approvedPaymentTypeCounts.mastercard,
          report: "paymentType",
          category: "mastercard"
        },
        {
          label: "Visa",
          total: paymentsByCreditCardType.visaCard,
          fees: feesByCreditCardType.visaCard,
          count: approvedPaymentTypeCounts.visaCard,
          report: "paymentType",
          category: "visaCard"
        }
      ]
    : [];

  const totalPayments =
    paymentsByCreditCardType
      ? totalCreditCardPayments(paymentsByCreditCardType)
      : 0;

  const totalFees =
    feesByCreditCardType
      ? totalCreditCardFees(feesByCreditCardType)
      : 0;

  const totalCount =
    approvedPaymentTypeCounts
        ? totalCreditCardTransactions(approvedPaymentTypeCounts)
        : 0;

  if (loading) {
    return (
      <Card sx={{ height: "100%", px: 2 }}>
        <CardContent>
          <Box
            sx={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              minHeight: 200
            }}
          >
            <CircularProgress />
          </Box>
        </CardContent>
      </Card>
    );
  }

  if (error) {
    return (
      <Card sx={{ height: "100%", px: 2 }}>
        <CardContent>
          <Typography
            sx={{ fontWeight: "bold", mb: 2 }}
            variant="h6"
          >
            Payments Details
          </Typography>
  
          <Alert severity="error">
            {error}
          </Alert>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card sx={{ height: "100%", px: 2 }}>
      <CardContent>
        <Typography
          sx={{
            fontWeight: "bold",
            mb: 2
          }}
          variant="h6"
        >
          Payment Details
        </Typography>
        
        <Grid
          container
          sx={{ mb: 1 }}
        >
          <Grid size={3}>
            <Typography 
              sx={{ fontWeight: "bold" }}
              align="left"
            >
              Payment Type
            </Typography>
          </Grid>

          <Grid size={3}>
            <Typography
              sx={{ fontWeight: "bold" }}
              align="right"
            >
              Total
            </Typography>
          </Grid>

          <Grid size={3}>
            <Typography
              sx={{ fontWeight: "bold" }}
              align="right"
            >
              Fees
            </Typography>
          </Grid>

          <Grid size={3}>
            <Typography
              sx={{ fontWeight: "bold" }}
              align="right"
            >
              # Subpoenas
            </Typography>
          </Grid>
        </Grid>

        {dataRows.map((dataRow, index) => (
          <Box key={dataRow.label}>
            {index === 4 && (
              <Divider sx={{ my: 1 }} />
            )}

            <PaymentTypeDataRow
              label={dataRow.label}
              total={dataRow.total}
              fees={dataRow.fees}
              count={dataRow.count}
              report={dataRow.report}
              category={dataRow.category}
            />
          </Box>
        ))}

        <Grid
          container
          sx={{ mt: 3 }}
        >
          <Grid size={3}>
            <Typography 
              sx={{ fontWeight: "bold" }}
              align="left"
            >
              Total
            </Typography>
          </Grid>

          <Grid size={3}>
            <Typography
              sx={{ fontWeight: "bold" }}
              align="right"
            >
              {formatCurrency(totalPayments)}
            </Typography>
          </Grid>

          <Grid size={3}>
            <Typography
              sx={{ fontWeight: "bold" }}
              align="right"
            >
              {formatCurrency(totalFees)}
            </Typography>
          </Grid>

          <Grid size={3}>
            <Typography
              sx={{ fontWeight: "bold" }}
              align="right"
            >
              {formatCount(totalCount)}
            </Typography>
          </Grid>
        </Grid>

      </CardContent>
    </Card>
  );
}