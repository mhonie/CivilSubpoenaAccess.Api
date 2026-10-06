import axios from "./axios";

import type { SubpoenaOrderDetail } from "../models/SubpoenaOrderDetail";

type SubpoenaOrderDetailResponse = {
  data?: {
    subpoenaOrderDetails?: SubpoenaOrderDetail | null;
  };
  errors?: { message: string }[];
};

export async function getSubpoenaOrderDetail(
  transactionId: string
): Promise<SubpoenaOrderDetail> {
  const query = `
    query SubpoenaOrderDetails($transactionId: String!) {
      subpoenaOrderDetails(transactionId: $transactionId) {
        transactionId
        status
        paymentType
        dateSubmitted
        username
        email
        reviewClerk
        actionDate
        messageToFiler
        convenienceFee
        totalFee
        subpoenas {
          subpoenaNumber
          caption
          caseId
          filingDate
          fee
          status
          subpoenaType
        }
        error
      }
    }
  `;

  const response = await axios.post<SubpoenaOrderDetailResponse>("/graphql", {
    query,
    variables: {
      transactionId
    }
  });

  if (response.data.errors?.length) {
    throw new Error(response.data.errors[0].message);
  }

  const subpoenaOrderDetail = response.data.data?.subpoenaOrderDetails;

  if (!subpoenaOrderDetail) {
    throw new Error("Subpoena order details were not returned by the API.");
  }

  if (subpoenaOrderDetail.error) {
    throw new Error(subpoenaOrderDetail.error);
  }

  return subpoenaOrderDetail;
}