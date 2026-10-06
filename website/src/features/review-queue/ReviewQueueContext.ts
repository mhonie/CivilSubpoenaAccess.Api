import { createContext, type Dispatch, type SetStateAction } from "react";
import type { GridSortModel } from "@mui/x-data-grid";

import type {
  SubpoenaOrderDetail
} from "../../models/SubpoenaOrderDetail";

import type {
  ReviewQueuePageInfo
} from "../../api/subpoenasForReview";

import type {
  ReviewQueueFilters
} from "../../models/ReviewQueueFilters";

import type {
  SubpoenasByTransaction
} from "../../models/SubpoenasByTransaction";


export type PagingRequest = {
  first?: number;
  after: string | null;
  last?: number;
  before: string | null;
};

export type ReviewQueueContextType = {
  rows: SubpoenasByTransaction[];
  setRows: Dispatch<SetStateAction<SubpoenasByTransaction[]>>;
  draftFilters: ReviewQueueFilters;
  setDraftFilters: Dispatch<SetStateAction<ReviewQueueFilters>>;
  appliedFilters: ReviewQueueFilters;
  setAppliedFilters: Dispatch<SetStateAction<ReviewQueueFilters>>;
  pageSize: number;
  setPageSize: Dispatch<SetStateAction<number>>;
  pageInfo: ReviewQueuePageInfo | null;
  setPageInfo: Dispatch<SetStateAction<ReviewQueuePageInfo | null>>;
  cursorHistory: (string | null)[];
  setCursorHistory: Dispatch<SetStateAction<(string | null)[]>>;
  pagingRequest: PagingRequest;
  setPagingRequest: Dispatch<SetStateAction<PagingRequest>>;
  sortModel: GridSortModel;
  setSortModel: Dispatch<SetStateAction<GridSortModel>>;

  expansionOverrides: Record<string, boolean>;

  setExpansionOverrides: Dispatch<
    SetStateAction<Record<string, boolean>>
  >;

  scrollPosition: number;
  setScrollPosition: Dispatch<SetStateAction<number>>;
  hasLoaded: boolean;
  setHasLoaded: Dispatch<SetStateAction<boolean>>;
  clearReviewQueue: () => void;

  updateCachedTransaction: (orderDetail: SubpoenaOrderDetail) => void;
};

export const ReviewQueueContext =

  createContext<ReviewQueueContextType | null>(null);