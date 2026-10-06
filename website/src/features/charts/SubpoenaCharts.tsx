import { useCallback } from "react";

import {
  Alert,
  Box,
  CircularProgress,
  Typography
} from "@mui/material";

import {
  getMonthlyPaymentTypeCounts,
  type PaymentTypeCountsByMonth
} from "../../api/subpoenaCounts";

import useSubpoenaCharts from "../../hooks/useSubpoenaCharts";

import {
  toUtcEndDate,
  toUtcStartDate
} from "../../utils/dateUtils";

import ChartFilters from "./ChartFilters";
import SubpoenasByOfflinePaymentType from
  "./SubpoenasByOfflinePaymentType";

import SubpoenasByOnlinePaymentType from
  "./SubpoenasByOnlinePaymentType";


type SubpoenaChartsProps = {
  title: string;
  filingStartDate: string;
  filingEndDate: string;
  dateError: string | null;
  filtersDisabled?: boolean;
  onFilingStartDateChange?: (value: string) => void;
  onFilingEndDateChange?: (value: string) => void;
};

export default function SubpoenaCharts
(
  {
    title,
    filingStartDate,
    filingEndDate,
    dateError,
    filtersDisabled = false,
    onFilingStartDateChange,
    onFilingEndDateChange
  }: SubpoenaChartsProps
)
{
  const loadPaymentTypeCounts = useCallback(
    async () =>
    {
      if (dateError)

        return {
          paymentTypeCountsByPeriod: []
        };

      return getMonthlyPaymentTypeCounts(
        toUtcStartDate(filingStartDate),

        toUtcEndDate(filingEndDate)
      );
    },

    [dateError, filingStartDate, filingEndDate]
  );

  const {
    data: monthlyPaymentTypeCounts,
    loading,
    error
  } = useSubpoenaCharts<PaymentTypeCountsByMonth>
  (
    loadPaymentTypeCounts,

    "Unable to load payment type information."
  );

  const paymentTypeCountsByPeriod =

    monthlyPaymentTypeCounts?.paymentTypeCountsByPeriod ?? [];

  return (
    <Box>
      <Typography variant="h5" sx={{mb: 3}}>
        {title}
      </Typography>

      <ChartFilters
        filingStartDate={filingStartDate}
        filingEndDate={filingEndDate}
        disabled={filtersDisabled}
        onFilingStartDateChange={onFilingStartDateChange}
        onFilingEndDateChange={onFilingEndDateChange}
      />

      {loading &&
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            height: 500
          }}
        >
          <CircularProgress />
        </Box>
      }

      {!loading && error &&
        <Alert severity="error">
          {error}
        </Alert>
      }

      {!loading && !error &&
        <>
          <SubpoenasByOnlinePaymentType
            paymentTypeCountsByPeriod={paymentTypeCountsByPeriod}
          />

          <SubpoenasByOfflinePaymentType
            paymentTypeCountsByPeriod={paymentTypeCountsByPeriod}
          />
        </>
      }
    </Box>
  );
}