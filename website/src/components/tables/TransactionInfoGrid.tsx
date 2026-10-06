import { DataGrid } from "@mui/x-data-grid";
import type { GridColDef } from "@mui/x-data-grid";
import {
  Box,
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select
} from "@mui/material";

import type { TransactionInfo } from "../../models/TransactionInfo";

import { formatCount, formatCurrency } from "../../utils/mathUtils";
import { formatDate } from "../../utils/dateUtils";

type TransactionInfoGridProps = {
  rows: TransactionInfo[];
  loading: boolean;
  dateHeader:
    | "Approved Date"
    | "Rejected Date"
    | "Filing Date";
  dateField:
    | "approvedDate"
    | "rejectedDate"
    | "filingDate";
  pageSize: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  onPageSizeChange: (pageSize: number) => void;
  onNextPage: () => void;
  onPreviousPage: () => void;
};

export default function TransactionInfoGrid({
  rows,
  loading,
  dateHeader,
  dateField,
  pageSize,
  hasNextPage,
  hasPreviousPage,
  onPageSizeChange,
  onNextPage,
  onPreviousPage
}: TransactionInfoGridProps) {

  const columns: GridColDef<TransactionInfo>[] = [
    {
      field: "transactionId",
      headerName: "Transaction ID",
      flex: 2,
    },
    {
      field: "itemCount",
      headerName: "Item Count",
      type: "number",
      flex: 1,
      align: "right",
      headerAlign: "right",
      valueFormatter: (value) => formatCount(value)
    },
    {
      field: "username",
      headerName: "Username",
      flex: 2
    },
    {
      field: dateField,
      headerName: dateHeader,
      type: "dateTime",
      flex: 2,
      valueFormatter: (value) => formatDate(value)
    },
    {
      field: "paymentAmount",
      headerName: "Payment Amount",
      type: "number",
      flex: 1.5,
      align: "right",
      headerAlign: "right",
      valueFormatter: (value) => formatCurrency(value)
    }
  ];

  return (
    <Box sx={{mb: 3}}>
      <DataGrid
        rows={rows}
        columns={columns}
        loading={loading}
        getRowId={(row) => row.transactionId}
        autoHeight
        disableColumnMenu
        disableRowSelectionOnClick
        hideFooter
        density="compact"
        sx={{
          "& .MuiDataGrid-row:nth-of-type(even)": {
            backgroundColor: "#fafafa"
          },
          "& .MuiDataGrid-columnHeaderTitle": {
            fontWeight: "bold"
          },
          mb: 3
        }}
      />

      <Box
        sx={{
          mt: 2,
          display: "flex",
          flexDirection: "row",
          justifyContent: "space-between",
          alignItems: "center"
        }}
      >
        <FormControl size="small" sx={{ width: 120 }}>
          <InputLabel>
            # Rows
          </InputLabel>

          <Select
            disabled={loading}
            label="# Rows"
            value={pageSize}
            onChange={(e) => onPageSizeChange(Number(e.target.value))}
          >
            <MenuItem value={25}>25</MenuItem>
            <MenuItem value={50}>50</MenuItem>
            <MenuItem value={100}>100</MenuItem>
          </Select>
        </FormControl>

        <Box
          sx={{
            display: "flex",
            gap: 2
          }}
        >
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