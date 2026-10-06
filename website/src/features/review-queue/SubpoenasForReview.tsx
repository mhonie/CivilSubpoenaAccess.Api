import { useEffect, useState } from "react";
import { Alert, Box } from "@mui/material";

import { getSubpoenasForReview } from "../../api/subpoenasForReview";

import ReviewQueueGrid from
  "../../components/tables/SubpoenaReviewGrid";

import type {
  SubpoenasByTransaction
} from "../../models/SubpoenasByTransaction";

import {
  initialReviewQueueFilters
} from "../../models/ReviewQueueFilters";

import { useReviewQueueContext } from "../../utils/contextUtils";

import {
  optionalDateRangeError,
  toUtcEndDate,
  toUtcStartDate
} from "../../utils/dateUtils";

import ReviewQueueFilters from "./ReviewQueueFilters";


export default function ReviewQueue()
{
  const {
    rows,
    setRows,
    draftFilters,
    setDraftFilters,
    appliedFilters,
    setAppliedFilters,
    pageSize,
    setPageSize,
    pageInfo,
    setPageInfo,
    cursorHistory,
    setCursorHistory,
    pagingRequest,
    setPagingRequest,
    sortModel,
    setSortModel,
    expansionOverrides,
    setExpansionOverrides,
    scrollPosition,
    setScrollPosition,
    hasLoaded,
    setHasLoaded
  } = useReviewQueueContext();

  const [loading, setLoading] = useState(false);

  const [error, setError] = useState<string | null>(null);

  const [validationError, setValidationError] =

    useState<string | null>(null);

  function resetPagination()
  {
    setHasLoaded(false);

    setCursorHistory([]);

    setPagingRequest({
      first: pageSize,
      after: null,
      last: undefined,
      before: null
    });
  }

  function applyFilters()
  {
    setError(null);

    const dateError = optionalDateRangeError(
      draftFilters.filingStartDate,

      draftFilters.filingEndDate,

      "Filing"
    )

    ?? optionalDateRangeError(
      draftFilters.reviewStartDate,

      draftFilters.reviewEndDate,

      "Review"
    );

    setValidationError(dateError);

    if (dateError)

      return;

    setAppliedFilters({...draftFilters});

    resetPagination();
  }

  function resetFilters()
  {
    const initialFilters = {...initialReviewQueueFilters};

    setDraftFilters(initialFilters);

    setAppliedFilters(initialFilters);

    setValidationError(null);

    setError(null);

    resetPagination();
  }

  useEffect(
    () =>
    {
      if (hasLoaded)

        return;

      let cancelled = false;

      async function load()
      {
        setLoading(true);

        setError(null);

        try
        {
          const result = await getSubpoenasForReview({
            status: appliedFilters.status,

            isPendingApproval: appliedFilters.isPendingApproval,

            isPendingPayment: appliedFilters.isPendingPayment,

            includeWebPayments: appliedFilters.includeWebPayments,

            orderNumber: appliedFilters.transactionNumber || null,

            filingStartDate: appliedFilters.filingStartDate

              ? toUtcStartDate(appliedFilters.filingStartDate)

              : null,

            filingEndDate: appliedFilters.filingEndDate

              ? toUtcEndDate(appliedFilters.filingEndDate)

              : null,

            caseId: appliedFilters.caseId || null,

            submitterEmail: appliedFilters.submitterEmail || null,

            reviewClerk: appliedFilters.reviewClerk || null,

            reviewStartDate: appliedFilters.reviewStartDate

              ? toUtcStartDate(appliedFilters.reviewStartDate)

              : null,

            reviewEndDate: appliedFilters.reviewEndDate

              ? toUtcEndDate(appliedFilters.reviewEndDate)

              : null,

            first: pagingRequest.first,

            after: pagingRequest.after,

            last: pagingRequest.last,

            before: pagingRequest.before,

            sort: sortModel[0]?.sort

              ? {
                  field: sortModel[0].field as keyof Omit<SubpoenasByTransaction, "subpoenas">,

                  direction: sortModel[0].sort.toUpperCase() as "ASC" | "DESC"
                }

              : null
          });

          if (!cancelled)
          {
            setRows(result.nodes);

            setPageInfo(result.pageInfo);

            setHasLoaded(true);
          }
        }
        catch (loadError)
        {
          if (!cancelled)
          {
            setRows([]);

            setPageInfo(null);

            setError(
              loadError instanceof Error

                ? loadError.message

                : "Unable to load transactions."
            );
          }
        }
        finally
        {
          if (!cancelled)

            setLoading(false);
        }
      }

      void load();

      return () =>
      {
        cancelled = true;
      };
    },

    [appliedFilters, pagingRequest, sortModel, hasLoaded, setRows, setPageInfo, setHasLoaded]
  );

  useEffect(
    () =>
    {
      const restoreScrollPosition = window.requestAnimationFrame(
        () => window.scrollTo({
          top: scrollPosition,

          left: 0,

          behavior: "auto"
        })
      );

      return () =>
      {
        window.cancelAnimationFrame(restoreScrollPosition);

        setScrollPosition(window.scrollY);
      };
    },

    [scrollPosition, setScrollPosition]
  );

  return (
    <Box sx={{mb: 2, px: 2}}>
      {validationError &&
        <Alert severity="error" sx={{mb: 3, textAlign: "left"}}>
          {validationError}
        </Alert>
      }

      {error &&
        <Alert severity="error" sx={{mb: 3, textAlign: "left"}}>
          {error}
        </Alert>
      }

      <ReviewQueueFilters
        filters={draftFilters}
        loading={loading}
        onChange={(filters) =>
        {
          setDraftFilters(filters);

          setValidationError(null);

          setError(null);
        }}
        onApply={applyFilters}
        onReset={resetFilters}
      />

      <ReviewQueueGrid
        rows={rows}
        loading={loading}
        pageSize={pageSize}
        hasNextPage={pageInfo?.hasNextPage ?? false}
        hasPreviousPage={cursorHistory.length > 0}
        expansionOverrides={expansionOverrides}
        onToggleTransaction={(transactionId, expanded) =>
          setExpansionOverrides(overrides => ({
            ...overrides,
      
            [transactionId]: !expanded
          }))
        }
        onPageSizeChange={(size) =>
        {
          setHasLoaded(false);

          setPageSize(size);

          setCursorHistory([]);

          setPagingRequest({
            first: size,
            after: null,
            last: undefined,
            before: null
          });
        }}
        onNextPage={() =>
        {
          if (!pageInfo?.endCursor)

            return;

          setHasLoaded(false);

          setCursorHistory(
            history =>
            [
              ...history,

              pageInfo.startCursor ?? null
            ]
          );

          setPagingRequest({
            first: pageSize,

            after: pageInfo.endCursor,

            last: undefined,

            before: null
          });
        }}
        onPreviousPage={() =>
        {
          if (!pageInfo?.startCursor)

            return;

          setHasLoaded(false);

          setCursorHistory(
            history => history.slice(0, -1)
          );

          setPagingRequest({
            first: undefined,
            after: null,
            last: pageSize,
            before: pageInfo.startCursor
          });
        }}
        sortModel={sortModel}
        onSortModelChange={(model) =>
        {
          setSortModel(model.slice(0, 1));

          resetPagination();
        }}
      />
    </Box>
  );
}