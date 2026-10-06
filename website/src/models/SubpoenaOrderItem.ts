export interface SubpoenaOrderItem {
  subpoenaNumber: number;
  caption: string | null;
  caseId: string | null;
  filingDate: string | null;
  fee: number;
  status: string | null;
  subpoenaType: string | null;
}