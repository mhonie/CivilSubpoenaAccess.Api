import { useCallback } from "react";

import { useReportsContext } from "../../utils/contextUtils";
import { toUtcEndDate, toUtcStartDate } from "../../utils/dateUtils";

import {
  getSubpoenaCountsByStatus,
  type SubpoenaCountsByStatus
} from "../../api/subpoenaCounts";

import useSubpoenaReports from "../../hooks/useSubpoenaReports";

import SubpoenaCountCard, {
  type SubpoenaCountDataRowModel
} from "../../components/cards/SubpoenaCountCard";

import { totalSubpoenaCountByStatus } from "../../utils/mathUtils";

export default function SubpoenaCountByStatusCard() {

  const {
    filingStartDate,
    filingEndDate,
    dateError,
    approvedStartDate,
    approvedEndDate
  } = useReportsContext();

  const loadStatus = useCallback(
    async () => {

      if (dateError) {
        return {
          approved: 0,
          rejected: 0,
          pendingPayment: 0,
          pendingApproval: 0
        };
      }

      return getSubpoenaCountsByStatus(
        toUtcStartDate(approvedStartDate),
        toUtcEndDate(approvedEndDate),
        toUtcStartDate(filingStartDate),
        toUtcEndDate(filingEndDate)
      );
    },
    [
      dateError,
      filingStartDate,
      filingEndDate,
      approvedStartDate,
      approvedEndDate
    ]
  );

  const {
    data: countsByStatus,
    loading,
    error
  } = useSubpoenaReports<SubpoenaCountsByStatus>(
    loadStatus,
    "Unable to load subpoena status information."
  );

  const dataRows: SubpoenaCountDataRowModel[] = countsByStatus
  ? [
      {
        label: "Approved",
        value: countsByStatus.approved,
        report: "subpoenaStatus",
        category: "approved"
      },
      {
        label: "Rejected",
        value: countsByStatus.rejected,
        report: "subpoenaStatus",
        category: "rejected"
      },
      {
        label: "Pending Payment",
        value: countsByStatus.pendingPayment,
        report: "subpoenaStatus",
        category: "pendingPayment"
      },
      {
        label: "Pending Approval",
        value: countsByStatus.pendingApproval,
        report: "subpoenaStatus",
        category: "pendingApproval"
      }
    ]
  : [];

  return (
    <SubpoenaCountCard
      title="Subpoenas by Status"
      loading={loading}
      error={error}
      dataRows={dataRows}
      total={
        countsByStatus
          ? totalSubpoenaCountByStatus(countsByStatus)
          : 0
      }
    />
  );
}