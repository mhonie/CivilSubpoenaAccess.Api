import { useEffect, useState } from "react";
import { Box, Button, Grid, Typography } from "@mui/material";
import { Link, useNavigate, useParams } from "react-router-dom";

import TransactionInfoGrid from "../../components/tables/TransactionInfoGrid";
import { useAuthenticationContext, useReportsContext } from "../../utils/contextUtils";
import { toUtcEndDate, toUtcStartDate } from "../../utils/dateUtils";
import { getTransactions, type TransactionPageInfo } from "../../api/transactionInfo";

import type { TransactionInfo } from "../../models/TransactionInfo";
import { reportTypeLabels, type ReportType } from "../../models/ReportType";
import { categoryLabels, type CategoryType } from "../../models/CategoryType";

import { getTransactionReportConfig } from "../../utils/reportUtils";
import TopBanner from "../../components/TopBanner";

type PagingRequest = {
  first?: number;
  after: string | null;
  last?: number;
  before: string | null;
};

export default function Transactions() {

  const {
    reportType: reportTypeParameter,
    category: categoryParameter
  } = useParams();

  const reportType = reportTypeParameter as ReportType;

  const category = categoryParameter as CategoryType;

  const reportTypeLabel = reportTypeLabels[reportType];

  const categoryLabel = categoryLabels[category];

  const {
    filingStartDate,
    filingEndDate,
    approvedStartDate,
    approvedEndDate
  } = useReportsContext();

  const {
    dateHeader,
    dateField
  } = getTransactionReportConfig(
    reportType,
    category
  );

  const [rows, setRows] = useState<TransactionInfo[]>([]);
  const [loading, setLoading] = useState(false);
  const [pageSize, setPageSize] = useState(25);
  const [pageInfo, setPageInfo] = useState<TransactionPageInfo | null>(null);
  const [cursorHistory, setCursorHistory] = useState<(string | null)[]>([]);
  const [pagingRequest, setPagingRequest] =
  
    useState<PagingRequest>({
      first: 25,
      after: null,
      last: undefined,
      before: null
    });

  useEffect(() => {

    if (!reportType || !category) {
      return;
    }

    let cancelled = false;

    async function load() {

      setLoading(true);

      try {

        const result = await getTransactions({
          report: reportType,
          category,
          approvedStartDate: toUtcStartDate(approvedStartDate),
          approvedEndDate: toUtcEndDate(approvedEndDate),
          filingStartDate: toUtcStartDate(filingStartDate),
          filingEndDate: toUtcEndDate(filingEndDate),
          first: pagingRequest.first,
          after: pagingRequest.after,
          last: pagingRequest.last,
          before: pagingRequest.before
        });

        if (!cancelled) {
          setRows(result.nodes);
          setPageInfo(result.pageInfo);
        }
      }
      finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    }

    void load();

    return () => { cancelled = true; };

  }, [
    reportType,
    category,
    approvedStartDate,
    approvedEndDate,
    filingStartDate,
    filingEndDate,
    pagingRequest
  ]);

  const navigate = useNavigate();

  const { clearAuthentication } = useAuthenticationContext();

  function logout() {
    clearAuthentication();

    navigate("/", {replace: true});
  }

  return (
    <>
      <TopBanner/>
      <Box sx={{mb: 2}}>
        <Box>
          <Typography variant="h5">
            Transactions
          </Typography>

          <Typography sx={{mb:3}}>
            {reportTypeLabel}: {categoryLabel}
          </Typography>
        </Box>

        <TransactionInfoGrid
          rows={rows}
          loading={loading}
          dateHeader={dateHeader}
          dateField={dateField}
          pageSize={pageSize}
          hasNextPage={pageInfo?.hasNextPage ?? false}
          hasPreviousPage={cursorHistory.length > 0}

          onPageSizeChange={(size) => {
            setPageSize(size);
            setCursorHistory([]);
            setPagingRequest({
              first: size,
              after: null,
              last: undefined,
              before: null
            });
          }}

          onNextPage={() => {
            if (!pageInfo?.endCursor) {
              return;
            }
            setCursorHistory(history => [
              ...history,
              pageInfo.startCursor ?? null
            ]);
            setPagingRequest({
              first: pageSize,
              after: pageInfo.endCursor,
              last: undefined,
              before: null
            });
          }}

          onPreviousPage={() => {
            if (!pageInfo?.startCursor) {
              return;
            }            
            setCursorHistory(history =>
              history.slice(0, -1)
            );            
            setPagingRequest({
              first: undefined,
              after: null,
              last: pageSize,
              before: pageInfo.startCursor
            });
          }}
        />
      </Box>
      <Box
      sx={{
        width: "100%",
        maxWidth: 1200,
        mx: "auto"
      }}
    >
      <Grid container spacing={2} sx={{my: 5}}>
        <Grid size={{xs: 12, md: 6}}>
          <Button
            fullWidth
            variant="outlined"
            color="error"
            size="large"
            component={Link}
            to={"/reports"}
            sx={{mb: 5}}
          >
            Back
          </Button>
        </Grid>

        <Grid size={{xs: 12, md: 6}}>
          <Button
            fullWidth
            variant="contained"
            color="error"
            size="large"
            onClick={logout}
            sx={{mb: 5}}
          >
            Log Out
          </Button>
        </Grid>
      </Grid>
    </Box>
    </>
  );
}