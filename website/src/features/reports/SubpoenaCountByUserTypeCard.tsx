import { useCallback } from "react";

import { useReportsContext } from "../../utils/contextUtils";
import { toUtcEndDate, toUtcStartDate } from "../../utils/dateUtils";

import {
  getUserTypeCounts,
  type SubpoenaCountByUserType
} from "../../api/subpoenaCounts";

import useSubpoenaReports from "../../hooks/useSubpoenaReports";

import SubpoenaCountCard, {
  type SubpoenaCountDataRowModel
} from "../../components/cards/SubpoenaCountCard";

import { totalUserTypeCount } from "../../utils/mathUtils";

export default function SubpoenaCountByUserTypeCard() {

  const {
    dateError,
    approvedStartDate,
    approvedEndDate
  } = useReportsContext();

  const loadUserTypes = useCallback(
    async () => {

      if (dateError) {
        return {
          public: 0,
          eFilingAttorney: 0,
          eFilingProSe: 0,
          cityLaw: 0,
          citySolicitor: 0,
          filingService: 0
        };
      }

      return getUserTypeCounts(
        toUtcStartDate(approvedStartDate),
        toUtcEndDate(approvedEndDate)
      );
    },
    [
      dateError,
      approvedStartDate,
      approvedEndDate
    ]
  );

  const {
    data: userTypeCounts,
    loading,
    error
  } = useSubpoenaReports<SubpoenaCountByUserType>(
    loadUserTypes,
    "Unable to load user type information."
  );

  const dataRows: SubpoenaCountDataRowModel[] = userTypeCounts
  ? [
      {
        label: "Public",
        value: userTypeCounts.public,
        report: "userType",
        category: "public"
      },
      {
        label: "eFiling Attorney",
        value: userTypeCounts.eFilingAttorney,
        report: "userType",
        category: "eFilingAttorney"
      },
      {
        label: "eFiling Pro Se",
        value: userTypeCounts.eFilingProSe,
        report: "userType",
        category: "eFilingProSe"
      },
      {
        label: "City Law",
        value: userTypeCounts.cityLaw,
        report: "userType",
        category: "cityLaw"
      },
      {
        label: "City Solicitor",
        value: userTypeCounts.citySolicitor,
        report: "userType",
        category: "citySolicitor"
      },
      {
        label: "Filing Service",
        value: userTypeCounts.filingService,
        report: "userType",
        category: "filingService"
      }
    ]
  : [];

  return (
    <SubpoenaCountCard
      title="Subpoenas by User Type"
      loading={loading}
      error={error}
      dataRows={dataRows}
      total={
        userTypeCounts
          ? totalUserTypeCount(userTypeCounts)
          : 0
      }
    />
  );
}