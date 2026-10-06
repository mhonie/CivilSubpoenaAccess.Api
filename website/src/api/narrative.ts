import axios from "./axios";


type NarrativeResponse = {
  transactionId: string;
  narrative: string | null;
};

export async function getNarrative(transactionId: string): Promise<string> {
  const response = await axios.get<NarrativeResponse>
  (
    `/narrative/${encodeURIComponent(transactionId)}`
  );

  return response.data.narrative ?? "";
}

export async function updateNarrative
(
  transactionId: string,

  narrative: string
): Promise<void>
{
  await axios.put(
    `/narrative/${encodeURIComponent(transactionId)}`,

    {
      narrative
    }
  );
}