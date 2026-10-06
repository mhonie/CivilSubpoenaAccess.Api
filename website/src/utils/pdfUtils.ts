import type { SubpoenaInfo } from "../models/SubpoenaInfo";


const updatedSignatureDate = Date.UTC(2014, 4, 29);

export function oldSubpoenaPdfUrl(
  transactionId: string,
  subpoenaInfo: SubpoenaInfo,
): string {
  const reviewDate = subpoenaInfo.reviewDate
    ? new Date(subpoenaInfo.reviewDate).getTime()
    : Number.NaN;

  const pdfScript = reviewDate >= updatedSignatureDate
    ? "SubpoenaStamps_Mod20140528.php"
    : "SubpoenaStamps.php";

  const pdfUrl = new URL(
    `/efsprod/stamppdf/${pdfScript}`,
    import.meta.env.VITE_SUBPOENA_PDF_BASE_URL
  );

  pdfUrl.searchParams.set("uid", transactionId);
  pdfUrl.searchParams.set("sn", subpoenaInfo.subpoenaNumber.toString());
  pdfUrl.searchParams.set("q", "true");

  return pdfUrl.toString();
}

export function newSubpoenaPdfUrl
(
  transactionId: string,
  subpoenaNumber: number,
  actionDate: string | null
): string
{
  const reviewDate = actionDate

    ? new Date(actionDate).getTime()

    : Number.NaN;

  const pdfScript = reviewDate >= updatedSignatureDate

    ? "SubpoenaStamps_Mod20140528.php"

    : "SubpoenaStamps.php";

  const pdfUrl = new URL
  (
    `/efsprod/stamppdf/${pdfScript}`,

    import.meta.env.VITE_SUBPOENA_PDF_BASE_URL
  );

  pdfUrl.searchParams.set("uid", transactionId);
  pdfUrl.searchParams.set("sn", subpoenaNumber.toString());
  pdfUrl.searchParams.set("q", "true");

  return pdfUrl.toString();
}