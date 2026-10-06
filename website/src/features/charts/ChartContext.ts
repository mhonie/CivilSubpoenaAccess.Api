import { createContext } from "react";


type ChartContextType = {
  filingStartDate: string;
  filingEndDate: string;

  previousYearFilingStartDate: string;
  previousYearFilingEndDate: string;

  dateError: string | null;

  setFilingStartDate: (value: string) => void;
  setFilingEndDate: (value: string) => void;
};

export const ChartContext = createContext<ChartContextType | null>(null);