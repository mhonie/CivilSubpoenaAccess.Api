import { useContext } from "react";

import { AuthenticationContext } from
  "../features/authentication/AuthenticationContext";

import { ChartContext } from "../features/charts/ChartContext";
import { ReportsContext } from "../features/reports/ReportsContext";

import { ReviewQueueContext } from
  "../features/review-queue/ReviewQueueContext";


export function useAuthenticationContext()
{
  const context = useContext(AuthenticationContext);

  if (!context)

    throw new Error
    (
      "useAuthenticationContext must be used inside AuthenticationContextProvider"
    );

  return context;
}

export function useReportsContext()
{
  const context = useContext(ReportsContext);

  if (!context)

    throw new Error
    (
      "useReportsContext must be used inside ReportsContextProvider"
    );

  return context;
}

export function useChartContext()
{
  const context = useContext(ChartContext);

  if (!context)

    throw new Error
    (
      "useChartContext must be used inside ChartContextProvider"
    );

  return context;
}

export function useReviewQueueContext()
{
  const context = useContext(ReviewQueueContext);

  if (!context)

    throw new Error
    (
      "useReviewQueueContext must be used inside ReviewQueueContextProvider"
    );

  return context;
}