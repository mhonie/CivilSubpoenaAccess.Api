export type CategoryType =
  | "approved"
  | "rejected"
  | "pendingPayment"
  | "pendingApproval"
  | "attend"
  | "produce"
  | "public"
  | "eFilingAttorney"
  | "eFilingProSe"
  | "cityLaw"
  | "citySolicitor"
  | "filingService"
  | "inFormaPauperis"
  | "walkIn"
  | "americanExpress"
  | "discoverCard"
  | "mastercard"
  | "visaCard";

export const categoryLabels: Record<CategoryType, string> = {
  approved: "Approved",
  rejected: "Rejected",
  pendingPayment: "Pending Payment",
  pendingApproval: "Pending Approval",
  attend: "Attend",
  produce: "Produce",
  public: "Public",
  eFilingAttorney: "eFiling Attorney",
  eFilingProSe: "eFiling Pro Se",
  cityLaw: "City Law",
  citySolicitor: "City Solicitor",
  filingService: "Filing Service",
  inFormaPauperis: "IFP",
  walkIn: "Walk-Ins",
  americanExpress: "AMEX",
  discoverCard: "Discover",
  mastercard: "Mastercard",
  visaCard: "Visa"
};