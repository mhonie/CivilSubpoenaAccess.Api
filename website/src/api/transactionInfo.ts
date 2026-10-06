import axios from "./axios";

import type { ReportType } from "../models/ReportType";
import type { TransactionInfo } from "../models/TransactionInfo";

type TransactionReportConfig = {
  rootField: string;
  usesFilingDates: boolean;
};

const transactionReportConfigs: Record<ReportType, TransactionReportConfig> = {
  subpoenaStatus: {
    rootField: "transactionsBySubpoenaStatus",
    usesFilingDates: true
  },

  subpoenaType: {
    rootField: "transactionsBySubpoenaType",
    usesFilingDates: false
  },

  userType: {
    rootField: "transactionsByUserType",
    usesFilingDates: false
  },

  paymentType: {
    rootField: "transactionsByPaymentType",
    usesFilingDates: false
  }
};

export type TransactionPageInfo = {
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  startCursor?: string | null;
  endCursor?: string | null;
};

export type TransactionConnection = {
  nodes: TransactionInfo[];
  pageInfo: TransactionPageInfo;
};

export type GetTransactionsRequest = {
  report: ReportType;
  category: string;
  approvedStartDate: string;
  approvedEndDate: string;
  filingStartDate: string;
  filingEndDate: string;
  first?: number;
  after?: string | null;
  last?: number;
  before?: string | null;
};

export async function getTransactions(
  request: GetTransactionsRequest
): Promise<TransactionConnection> {

  const config = transactionReportConfigs[request.report];

  const connectionSelection = `
    nodes {
      transactionId
      username
      approvedDate
      rejectedDate
      filingDate
      itemCount
      paymentAmount
    }
    pageInfo {
      hasNextPage
      hasPreviousPage
      startCursor
      endCursor
    }
  `;

  const pagingArguments = `
    first: $first
    after: $after
    last: $last
    before: $before
  `;

  const query = config.usesFilingDates
    ? `
      query(
        $approvedStartDate: DateTime!
        $approvedEndDate: DateTime!
        $filingStartDate: DateTime!
        $filingEndDate: DateTime!
        $first: Int
        $after: String
        $last: Int
        $before: String
      ) {
        ${config.rootField}(
          approvedStartDate: $approvedStartDate
          approvedEndDate: $approvedEndDate
          filingStartDate: $filingStartDate
          filingEndDate: $filingEndDate
        ) {
          ${request.category}(
            ${pagingArguments}
          ) {
            ${connectionSelection}
          }
        }
      }
    `
    : `
      query(
        $approvedStartDate: DateTime!
        $approvedEndDate: DateTime!
        $first: Int
        $after: String
        $last: Int
        $before: String
      ) {
        ${config.rootField}(
          approvedStartDate: $approvedStartDate
          approvedEndDate: $approvedEndDate
        ) {
          ${request.category}(
            ${pagingArguments}
          ) {
            ${connectionSelection}
          }
        }
      }
    `;

  const response = await axios.post("/graphql", {
    query,
    variables: config.usesFilingDates
      ? {
          approvedStartDate: request.approvedStartDate,
          approvedEndDate: request.approvedEndDate,
          filingStartDate: request.filingStartDate,
          filingEndDate: request.filingEndDate,
          first: request.first,
          after: request.after,
          last: request.last,
          before: request.before
        }
      : {
          approvedStartDate: request.approvedStartDate,
          approvedEndDate: request.approvedEndDate,
          first: request.first,
          after: request.after,
          last: request.last,
          before: request.before
        }
  });

  if (response.data.errors?.length) {
    throw new Error(response.data.errors[0].message);
  }

  return response.data.data[config.rootField][request.category];
}