import { Grid, Typography } from "@mui/material";

import { Link } from "react-router-dom";

import type { ReportType } from "../../models/ReportType";
import { formatCount, formatCurrency } from "../../utils/mathUtils";
import type { CategoryType } from "../../models/CategoryType";

export type PaymentTypeDataRowModel = {
  label: string;
  total: number;
  fees: number;
  count: number;
  report: ReportType;
  category: CategoryType;
};

type PaymentTypeDataRowProps = {
  label: string;
  total: number;
  fees: number;
  count: number;
  report: ReportType;
  category: CategoryType;
};

export function PaymentTypeDataRow({
  label,
  total,
  fees,
  count,
  report,
  category
}: PaymentTypeDataRowProps) {

  return (
    <Grid container>
      <Grid 
        size={3}
        sx={{
          display: "flex",
          justifyContent: "flex-start"
        }}>
        <Typography 
          component={Link}
          to={`/reports/transactions/${report}/${category}`}
          sx={{
            color: "primary.main",
            textDecoration: "none",
            cursor: "pointer",
            "&:hover": {
              textDecoration: "underline"
            }
          }}>
          {label}
        </Typography>
      </Grid>

      <Grid
        size={3}
        sx={{
          display: "flex",
          justifyContent: "flex-end"
        }}
      >
        <Typography
        >
          {formatCurrency(total)}
        </Typography>
      </Grid>

      <Grid size={3}>
        <Typography align="right">
          {formatCurrency(fees)}
        </Typography>
      </Grid>

      <Grid size={3}>
        <Typography align="right">
          {formatCount(count)}
        </Typography>
      </Grid>
    </Grid>
  );
}