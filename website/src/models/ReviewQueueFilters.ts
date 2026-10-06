import { oneWeekAgo, today } from "../utils/dateUtils";

export type ReviewQueueFilters = {
  status: string | null;
  isPendingApproval: boolean;
  isPendingPayment: boolean;
  includeWebPayments: boolean;
  transactionNumber: string;
  filingStartDate: string;
  filingEndDate: string;
  caseId: string;
  submitterEmail: string;
  reviewClerk: string;
  reviewStartDate: string;
  reviewEndDate: string;
};

export const initialReviewQueueFilters: ReviewQueueFilters = {
  status: "All",
  isPendingApproval: false,
  isPendingPayment: false,
  includeWebPayments: false,
  transactionNumber: "",
  filingStartDate: oneWeekAgo(),
  filingEndDate: today(),
  caseId: "",
  submitterEmail: "",
  reviewClerk: "",
  reviewStartDate: oneWeekAgo(),
  reviewEndDate: today()
};
