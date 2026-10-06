import { useCallback } from "react";

import { useReportsContext } from "../../utils/contextUtils";
import { toUtcEndDate, toUtcStartDate } from "../../utils/dateUtils";

import {
  getSubpoenaCountsByType,
  type SubpoenaCountsByType
} from "../../api/subpoenaCounts";

import useSubpoenaReports from "../../hooks/useSubpoenaReports";

import SubpoenaCountCard, {
  type SubpoenaCountDataRowModel
} from "../../components/cards/SubpoenaCountCard";

import { totalSubpoenaCountByType } from "../../utils/mathUtils";

export default function SubpoenaCountByTypeCard() {

  const {
    dateError,
    approvedStartDate,
    approvedEndDate
  } = useReportsContext();

  const loadTypes = useCallback(
    async () => {

      if (dateError) {
        return {
          attend: 0,
          produce: 0
        };
      }

      return getSubpoenaCountsByType(
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
    data: countsByType,
    loading,
    error
  } = useSubpoenaReports<SubpoenaCountsByType>(
    loadTypes,
    "Unable to load subpoena type information."
  );

  const dataRows: SubpoenaCountDataRowModel[] = countsByType
  ? [
      {
        label: "Attend",
        value: countsByType.attend,
        report: "subpoenaType",
        category: "attend"
      },
      {
        label: "Produce",
        value: countsByType.produce,
        report: "subpoenaType",
        category: "produce"
      }
    ]
  : [];

  return (
    <SubpoenaCountCard
      title="Subpoenas by Type"
      loading={loading}
      error={error}
      dataRows={dataRows}
      total={
        countsByType
          ? totalSubpoenaCountByType(countsByType)
          : 0
      }
    />
  );
}