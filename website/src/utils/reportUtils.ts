import type { ReportType } from "../models/ReportType";

export type TransactionReportConfig = {
  dateHeader:
    | "Approved Date"
    | "Rejected Date"
    | "Filing Date";

  dateField:
    | "approvedDate"
    | "rejectedDate"
    | "filingDate";
};

export function getTransactionReportConfig(
  report: ReportType,
  category: string
): TransactionReportConfig {

  switch (report) {

    case "subpoenaStatus":

      switch (category) {

        case "approved":
          return {
            dateHeader: "Approved Date",
            dateField: "approvedDate"
          };

        case "rejected":
          return {
            dateHeader: "Rejected Date",
            dateField: "rejectedDate"
          };

        case "pendingApproval":
        case "pendingPayment":
          return {
            dateHeader: "Filing Date",
            dateField: "filingDate"
          };
      }

      break;

    case "subpoenaType":
    case "userType":
    case "paymentType":

      return {
        dateHeader: "Approved Date",
        dateField: "approvedDate"
      };
  }

  throw new Error(
    `Unsupported report/category: ${report}/${category}`
  );
}