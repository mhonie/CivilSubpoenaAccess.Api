import { useEffect, useState } from "react";

type ReportResult = {
  error?: string;
};

export default function useSubpoenaReports<T extends ReportResult>(
  loadData: () => Promise<T>,
  loadErrorMessage: string
) {

  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function load() {
      try {
        setLoading(true);
        setError(null);

        const result = await loadData();

        if (result.error) {
          console.error(result.error);
          setError(loadErrorMessage);
          setData(null);
          return;
        }

        setData(result);
      }
      catch (err) {
        console.error(err);
        setError(loadErrorMessage);
        setData(null);
      }
      finally {
        setLoading(false);
      }
    }

    load();

  }, [loadData, loadErrorMessage]);

  return {
    data,
    loading,
    error
  };
}