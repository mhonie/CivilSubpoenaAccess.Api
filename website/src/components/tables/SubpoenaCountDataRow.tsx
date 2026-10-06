import { Box, Typography } from "@mui/material";

import { Link } from "react-router-dom";

import type { ReportType } from "../../models/ReportType";
import { formatCount } from "../../utils/mathUtils";
import type { CategoryType } from "../../models/CategoryType";

type SubpoenaCountDataRowProps = {
  label: string;
  value: number;
  report: ReportType;
  category: CategoryType;
};

export default function SubpoenaCountDataRow({
  label,
  value,
  report,
  category
}: SubpoenaCountDataRowProps) {
  return (
    <Box sx={{ display: "flex", justifyContent: "space-between" }}>
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

      <Typography
      >
        {formatCount(value)}
      </Typography>
    </Box>
  );
}