import type { SubpoenaInfo } from "./SubpoenaInfo";

export interface SubpoenasByTransaction {
  transactionId: string;
  username: string | null;
  email: string | null;
  paymentType: string | null;
  subpoenas: SubpoenaInfo[];
}