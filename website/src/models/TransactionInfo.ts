export type TransactionInfo = {
  transactionId: string;
  username: string;
  approvedDate?: string;
  rejectedDate?: string;
  itemCount: number;
  paymentAmount: number;
};