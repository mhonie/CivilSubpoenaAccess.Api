import { useState, type ReactNode } from "react";
import { ReportsContext } from "./ReportsContext";
import { areValidReportDates, firstDayOfCurrentYear, today } from "../../utils/dateUtils";

export function ReportsContextProvider(
    { children }: { children: ReactNode }
) {
    const [filingStartDate, setFilingStartDate] = useState(firstDayOfCurrentYear());
    const [filingEndDate, setFilingEndDate] = useState(today());
    const [approvedStartDate, setApprovedStartDate] = useState(firstDayOfCurrentYear());
    const [approvedEndDate, setApprovedEndDate] = useState(today());

    const dateError =
      areValidReportDates(
        filingStartDate,
        filingEndDate,
        approvedStartDate,
        approvedEndDate
      )
        ? null
        : "Filing / review dates cannot be future dates. Year cannot be before 1900.";

    return (
      <ReportsContext.Provider
        value={{
          filingStartDate,
          filingEndDate,
          approvedStartDate,
          approvedEndDate,
          dateError,
          setFilingStartDate,
          setFilingEndDate,
          setApprovedStartDate,
          setApprovedEndDate
        }}
    >
        {children}
      </ReportsContext.Provider>
  );
}