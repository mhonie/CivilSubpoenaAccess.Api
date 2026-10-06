import { createContext } from "react";

type ReportsContextType = {
    filingStartDate: string;
    filingEndDate: string;
    approvedStartDate: string;
    approvedEndDate: string;

    dateError: string | null;

    setFilingStartDate: (value: string) => void;
    setFilingEndDate: (value: string) => void;
    setApprovedStartDate: (value: string) => void;
    setApprovedEndDate: (value: string) => void;
};

export const ReportsContext = createContext<ReportsContextType | null>(null);