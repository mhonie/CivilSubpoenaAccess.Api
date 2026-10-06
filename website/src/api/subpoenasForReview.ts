import axios from "./axios";

import type { SubpoenasByTransaction } from "../models/SubpoenasByTransaction";

export type ReviewQueuePageInfo = {
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  startCursor?: string | null;
  endCursor?: string | null;
};

export type ReviewQueueConnection = {
  nodes: SubpoenasByTransaction[];
  pageInfo: ReviewQueuePageInfo;
};

export type ReviewQueueSort = {
  field: keyof Omit<SubpoenasByTransaction, "subpoenas">;
  direction: "ASC" | "DESC";
};

export type GetReviewQueueRequest = {
  status?: string | null;
  isPendingApproval?: boolean | null;
  isPendingPayment?: boolean | null;
  includeWebPayments: boolean;
  orderNumber?: string | null;
  filingStartDate?: string | null;
  filingEndDate?: string | null;
  caseId?: string | null;
  submitterEmail?: string | null;
  reviewClerk?: string | null;
  reviewStartDate?: string | null;
  reviewEndDate?: string | null;
  first?: number;
  after?: string | null;
  last?: number;
  before?: string | null;
  sort?: ReviewQueueSort | null;
};

type ReviewQueueResponse = {
  data?: {
    subpoenasForReview?: {
      transactions?: ReviewQueueConnection | null;
      error?: string | null;
    } | null;
  };
  errors?: { message: string }[];
};

export async function getSubpoenasForReview(
  request: GetReviewQueueRequest
): Promise<ReviewQueueConnection> {

  const query = `
    query(
      $status: String
      $isPendingApproval: Boolean
      $isPendingPayment: Boolean
      $includeWebPayments: Boolean!
      $orderNumber: String
      $filingStartDate: DateTime
      $filingEndDate: DateTime
      $caseId: String
      $submitterEmail: String
      $reviewClerk: String
      $reviewStartDate: DateTime
      $reviewEndDate: DateTime
      $first: Int
      $after: String
      $last: Int
      $before: String
      $order: [SubpoenasByTransactionSortInput!]
    ) {
      subpoenasForReview(
        queryParameters: {
          status: $status
          isPendingApproval: $isPendingApproval
          isPendingPayment: $isPendingPayment
          includeWebPayments: $includeWebPayments
          orderNumber: $orderNumber
          filingStartDate: $filingStartDate
          filingEndDate: $filingEndDate
          caseId: $caseId
          submitterEmail: $submitterEmail
          reviewClerk: $reviewClerk
          reviewStartDate: $reviewStartDate
          reviewEndDate: $reviewEndDate
        }
      ) {
        transactions(
          first: $first
          after: $after
          last: $last
          before: $before
          order: $order
        ) {
          nodes {
            transactionId
            username
            email
            paymentType
            subpoenas {
              subpoenaNumber
              caption
              caseId
              subpoenaType
              filingDate
              status
              reviewClerk
              reviewDate
            }
          }
          pageInfo {
            hasNextPage
            hasPreviousPage
            startCursor
            endCursor
          }
        }
        error
      }
    }
  `;

  const response = await axios.post<ReviewQueueResponse>("/graphql", {
    query,
    variables: {
      status: request.status,
      isPendingApproval: request.isPendingApproval,
      isPendingPayment: request.isPendingPayment,
      includeWebPayments: request.includeWebPayments,
      orderNumber: request.orderNumber,
      filingStartDate: request.filingStartDate,
      filingEndDate: request.filingEndDate,
      caseId: request.caseId,
      submitterEmail: request.submitterEmail,
      reviewClerk: request.reviewClerk,
      reviewStartDate: request.reviewStartDate,
      reviewEndDate: request.reviewEndDate,
      first: request.first,
      after: request.after,
      last: request.last,
      before: request.before,
      order: request.sort
        ? [{[request.sort.field]: request.sort.direction}]
        : null
    }
  });

  if (response.data.errors?.length) {
    throw new Error(response.data.errors[0].message);
  }

  const result = response.data.data?.subpoenasForReview;

  if (result?.error) {
    throw new Error(result.error);
  }

  if (!result?.transactions) {
    throw new Error("Subpoena details were not returned by the API.");
  }

  return result.transactions;
}
