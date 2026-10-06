import { useState, type ReactNode } from "react";

import {
  areValidChartDates,
  firstDayOfCurrentYear,
  sameDateInPreviousYear,
  today
} from "../../utils/dateUtils";

import { ChartContext } from "./ChartContext";


export function ChartContextProvider
(
  {
    children
  }:
  {
    children: ReactNode;
  }
)
{
  const [filingStartDate, setFilingStartDate] =

    useState(firstDayOfCurrentYear());

  const [filingEndDate, setFilingEndDate] =

    useState(today());

  const previousYearFilingStartDate =

    sameDateInPreviousYear(filingStartDate);

  const previousYearFilingEndDate =

    sameDateInPreviousYear(filingEndDate);

  const dateError = areValidChartDates(
    filingStartDate,
    filingEndDate
  )

    ? null

    : "Filing dates cannot be future dates. Year cannot be before 1900.";

  return (
    <ChartContext.Provider
      value={{
        filingStartDate,
        filingEndDate,
        previousYearFilingStartDate,
        previousYearFilingEndDate,
        dateError,
        setFilingStartDate,
        setFilingEndDate
      }}
    >
      {children}
    </ChartContext.Provider>
  );
}