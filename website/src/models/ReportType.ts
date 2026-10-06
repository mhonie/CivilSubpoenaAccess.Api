export type ReportType =
  | "subpoenaStatus"
  | "subpoenaType"
  | "userType"
  | "paymentType";

export const reportTypeLabels: Record<ReportType, string> = {
  subpoenaStatus: "Subpoena Status",
  subpoenaType: "Subpoena Type",
  userType: "User Type",
  paymentType: "Payment Type"
};