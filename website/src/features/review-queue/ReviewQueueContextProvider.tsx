import { useState, type ReactNode } from "react";
import type { GridSortModel } from "@mui/x-data-grid";

import type {
  ReviewQueuePageInfo
} from "../../api/subpoenasForReview";

import type {
  SubpoenaOrderDetail
} from "../../models/SubpoenaOrderDetail";

import {
  initialReviewQueueFilters,
  type ReviewQueueFilters
} from "../../models/ReviewQueueFilters";

import type {
  SubpoenasByTransaction
} from "../../models/SubpoenasByTransaction";

import {
  ReviewQueueContext,
  type PagingRequest
} from "./ReviewQueueContext";


const initialPageSize = 25;

function initialPagingRequest(): PagingRequest
{
  return {
    first: initialPageSize,
    after: null,
    last: undefined,
    before: null
  };
}

export function ReviewQueueContextProvider
(
  {
    children
  }:
  {
    children: ReactNode;
  }
)
{
  const [rows, setRows] = useState<SubpoenasByTransaction[]>([]);

  const [draftFilters, setDraftFilters] = useState<ReviewQueueFilters>
  (
    {...initialReviewQueueFilters}
  );

  const [appliedFilters, setAppliedFilters] = useState<ReviewQueueFilters>
  (
    {...initialReviewQueueFilters}
  );

  const [pageSize, setPageSize] = useState(initialPageSize);

  const [pageInfo, setPageInfo] =

    useState<ReviewQueuePageInfo | null>(null);

  const [cursorHistory, setCursorHistory] =

    useState<(string | null)[]>([]);

  const [pagingRequest, setPagingRequest] = useState<PagingRequest>
  (
    initialPagingRequest()
  );

  const [sortModel, setSortModel] = useState<GridSortModel>([]);

  const [expansionOverrides, setExpansionOverrides] =

    useState<Record<string, boolean>>({});

  const [scrollPosition, setScrollPosition] = useState(0);

  const [hasLoaded, setHasLoaded] = useState(false);

  function updateCachedTransaction(orderDetail: SubpoenaOrderDetail)
  {
    const updatedSubpoenas = new Map
    (
      orderDetail.subpoenas.map(
        subpoena => [subpoena.subpoenaNumber, subpoena]
      )
    );

    setRows(currentRows => currentRows.map(transaction =>
    {
      if (transaction.transactionId != orderDetail.transactionId)

        return transaction;

      return {
        ...transaction,

        paymentType: orderDetail.paymentType,

        subpoenas: transaction.subpoenas.map(subpoena =>
        {
          const updatedSubpoena = updatedSubpoenas.get(subpoena.subpoenaNumber);

          if (!updatedSubpoena)

            return subpoena;

          return {
            ...subpoena,

            status: updatedSubpoena.status,

            reviewClerk: orderDetail.reviewClerk,

            reviewDate: orderDetail.actionDate
          };
        })
      };
    }));
  }

  function clearReviewQueue()
  {
    setRows([]);
    setDraftFilters({...initialReviewQueueFilters});
    setAppliedFilters({...initialReviewQueueFilters});
    setPageSize(initialPageSize);
    setPageInfo(null);
    setCursorHistory([]);
    setPagingRequest(initialPagingRequest());
    setSortModel([]);
    setExpansionOverrides({});
    setScrollPosition(0);
    setHasLoaded(false);
  }

    return (
      <ReviewQueueContext.Provider
      value={{
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
        setHasLoaded,
        updateCachedTransaction,
        clearReviewQueue
      }}
    >
      {children}
    </ReviewQueueContext.Provider>
    );
  }