import axios from "./axios";


export type SubpoenaApproval = {
  subpoenaNumber: number;
  approvalIndicator: "A" | "R";
};

export async function updateApprovals
(
  transactionId: string,

  approvals: SubpoenaApproval[]
): Promise<void>
{
  await axios.put  (
    `/approval/${encodeURIComponent(transactionId)}`,

    {
      approvals
    }
  );
}