import type { SubpoenaOrderItem } from "./SubpoenaOrderItem";

export interface SubpoenaOrderDetail {
  transactionId: string;
  status: string | null;
  paymentType: string | null;
  dateSubmitted: string | null;
  username: string | null;
  email: string | null;
  reviewClerk: string | null;
  actionDate: string | null;
  messageToFiler: string | null;
  convenienceFee: number;
  totalFee: number;
  subpoenas: SubpoenaOrderItem[];
  error: string | null;
}