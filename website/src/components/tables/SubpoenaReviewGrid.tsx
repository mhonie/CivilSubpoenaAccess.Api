import { Link as RouterLink } from "react-router-dom";
import type { GridSortModel } from "@mui/x-data-grid";
import KeyboardArrowDownIcon from "@mui/icons-material/KeyboardArrowDown";
import KeyboardArrowUpIcon from "@mui/icons-material/KeyboardArrowUp";
import {
  Box,
  Button,
  CircularProgress,
  Collapse,
  FormControl,
  IconButton,
  InputLabel,
  Link as MuiLink,
  MenuItem,
  Paper,
  Select,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TableSortLabel
} from "@mui/material";

import type { SubpoenasByTransaction } from "../../models/SubpoenasByTransaction";

import { formatDate } from "../../utils/dateUtils";
import { oldSubpoenaPdfUrl } from "../../utils/pdfUtils";

type ReviewQueueGridProps = {
  rows: SubpoenasByTransaction[];
  loading: boolean;
  pageSize: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  sortModel: GridSortModel;
  expansionOverrides: Record<string, boolean>;
  onPageSizeChange: (pageSize: number) => void;
  onSortModelChange: (sortModel: GridSortModel) => void;
  onToggleTransaction: (
    transactionId: string,

    expanded: boolean
  ) => void;
  onNextPage: () => void;
  onPreviousPage: () => void;
};

type TransactionColumn = {
  field: keyof Omit<SubpoenasByTransaction, "subpoenas">;
  headerName: string;
};

const transactionColumns: TransactionColumn[] = [
  {
    field: "transactionId",
    headerName: "Transaction #"
  },
  {
    field: "username",
    headerName: "Username"
  },
  {
    field: "email",
    headerName: "Submitter Email"
  },
  {
    field: "paymentType",
    headerName: "Payment Type"
  }
];

function SubpoenaRows
(
  {
    transaction
  }:
  {
    transaction: SubpoenasByTransaction;
  }
)
{
  return (
    <Box sx={{p: 2}}>
      <Table
        size="small"
        aria-label={`Subpoenas for ${transaction.transactionId}`}
      >
        <TableHead>
          <TableRow>
            <TableCell>Subpoena #</TableCell>
            <TableCell>Caption</TableCell>
            <TableCell>Case ID</TableCell>
            <TableCell>Subpoena Type</TableCell>
            <TableCell>Filing Date</TableCell>
            <TableCell>Status</TableCell>
            <TableCell>Review Clerk</TableCell>
            <TableCell>Review Date</TableCell>
          </TableRow>
        </TableHead>

        <TableBody>
          {transaction.subpoenas.map(subpoenaInfo =>
            <TableRow key={subpoenaInfo.subpoenaNumber}>
              <TableCell>
                {subpoenaInfo.subpoenaNumber}
              </TableCell>

              <TableCell sx={{width: 550}}>
                {(subpoenaInfo.caption ?? "").slice(0, 50)}
                {(subpoenaInfo.caption?.length ?? 0) >= 50 ? "..." : ""}
              </TableCell>

              <TableCell>
                {subpoenaInfo.caseId}
              </TableCell>

              <TableCell>
                {subpoenaInfo.subpoenaType
                  ?
                    <MuiLink
                      href={oldSubpoenaPdfUrl(
                        transaction.transactionId,

                        subpoenaInfo
                      )}
                      target="_blank"
                      rel="noopener noreferrer"
                    >
                      {subpoenaInfo.subpoenaType}
                    </MuiLink>
                  : null
                }
              </TableCell>

              <TableCell>
                {formatDate(subpoenaInfo.filingDate)}
              </TableCell>

              <TableCell>
                {subpoenaInfo.status}
              </TableCell>

              <TableCell>
                {subpoenaInfo.reviewClerk}
              </TableCell>

              <TableCell>
                {formatDate(subpoenaInfo.reviewDate)}
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </Box>
  );
}

export default function ReviewQueueGrid
(
  {
    rows,
    loading,
    pageSize,
    hasNextPage,
    hasPreviousPage,
    sortModel,
    expansionOverrides,
    onPageSizeChange,
    onSortModelChange,
    onToggleTransaction,
    onNextPage,
    onPreviousPage
  }: ReviewQueueGridProps
)
{
  function changeSort(column: TransactionColumn) {
    const currentSort = sortModel[0];

    onSortModelChange(
      [
        {
          field: column.field,
          sort: currentSort?.field == column.field
                && currentSort.sort == "asc"

            ? "desc"

            : "asc"
        }
      ]
    );
  }

  return (
    <Box sx={{mb: 3}}>
      <TableContainer component={Paper}>
        <Table aria-label="Subpoena transactions">
          <TableHead>
            <TableRow>
              <TableCell padding="checkbox" />

              {transactionColumns.map(column =>
                <TableCell key={column.field}>
                  <TableSortLabel
                    active={sortModel[0]?.field == column.field}
                    direction={
                      sortModel[0]?.field == column.field

                        ? sortModel[0].sort ?? "asc"

                        : "asc"
                    }
                    onClick={() => changeSort(column)}
                  >
                    {column.headerName}
                  </TableSortLabel>
                </TableCell>
              )}
            </TableRow>
          </TableHead>

          <TableBody>
            {loading &&
              <TableRow>
                <TableCell colSpan={5} align="center">
                  <CircularProgress size={24} sx={{mr: 2}} />

                  Loading transactions...
                </TableCell>
              </TableRow>
            }

            {!loading && rows.length == 0 &&
              <TableRow>
                <TableCell colSpan={5} align="center">
                  No transactions found.
                </TableCell>
              </TableRow>
            }

            {!loading && rows.map((transaction, index) => {
              const expanded = expansionOverrides[transaction.transactionId]

                ?? index == 0;

              return [
                <TableRow
                  hover
                  key={transaction.transactionId}
                  onClick={() => onToggleTransaction(
                    transaction.transactionId,
                  
                    expanded
                  )}
                  sx={{
                    cursor: "pointer",
                    backgroundColor: index % 2 == 0 ? "inherit" : "#fafafa"
                  }}
                >
                  <TableCell padding="checkbox">
                    <IconButton
                      size="small"
                      aria-label={
                        expanded

                          ? "Collapse transaction"

                          : "Expand transaction"
                      }
                    >
                      {expanded

                        ? <KeyboardArrowUpIcon />

                        : <KeyboardArrowDownIcon />
                      }
                    </IconButton>
                  </TableCell>

                  <TableCell>
                    <MuiLink
                      component={RouterLink}
                      to={`/queue/${encodeURIComponent(
                        transaction.transactionId
                      )}`}
                      onClick={(event) => event.stopPropagation()}
                    >
                      {transaction.transactionId}
                    </MuiLink>
                  </TableCell>

                  <TableCell>
                    {transaction.username}
                  </TableCell>

                  <TableCell>
                    {transaction.email}
                  </TableCell>

                  <TableCell>
                    {transaction.paymentType}
                  </TableCell>
                </TableRow>,

                <TableRow key={`${transaction.transactionId}-subpoenas`}>
                  <TableCell colSpan={5} sx={{p: 0}}>
                    <Collapse in={expanded} timeout="auto" unmountOnExit>
                      <SubpoenaRows transaction={transaction} />
                    </Collapse>
                  </TableCell>
                </TableRow>
              ];
            })}
          </TableBody>
        </Table>
      </TableContainer>

      <Box
        sx={{
          mt: 2,
          display: "flex",
          flexDirection: "row",
          justifyContent: "space-between",
          alignItems: "center"
        }}
      >
        <FormControl size="small" sx={{width: 120}}>
          <InputLabel>
            # Rows
          </InputLabel>

          <Select
            disabled={loading}
            label="# Rows"
            value={pageSize}
            onChange={(event) => onPageSizeChange(Number(event.target.value))}
          >
            <MenuItem value={25}>25</MenuItem>
            <MenuItem value={50}>50</MenuItem>
            <MenuItem value={100}>100</MenuItem>
          </Select>
        </FormControl>

        <Box sx={{display: "flex", gap: 2}}>
          <Button
            variant="outlined"
            onClick={onPreviousPage}
            disabled={!hasPreviousPage || loading}
          >
            Prev
          </Button>

          <Button
            variant="contained"
            onClick={onNextPage}
            disabled={!hasNextPage || loading}
          >
            Next
          </Button>
        </Box>
      </Box>
    </Box>
  );
}