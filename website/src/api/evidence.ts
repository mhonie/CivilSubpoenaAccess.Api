import axios from "./axios";


type EvidenceResponse = {
  transactionId: string;
  subpoenaNumber: number;
  evidence: string | null;
};

export async function getEvidence
(
  transactionId: string,

  subpoenaNumber: number
): Promise<string>
{
  const response = await axios.get<EvidenceResponse>
  (
    `/evidence/${encodeURIComponent(transactionId)}/${subpoenaNumber}`
  );

  return response.data.evidence ?? "";
}

export async function updateEvidence
(
  transactionId: string,

  subpoenaNumber: number,

  evidence: string
): Promise<void>
{
  await axios.put(
    `/evidence/${encodeURIComponent(transactionId)}/${subpoenaNumber}`,

    {
      evidence
    }
  );
}