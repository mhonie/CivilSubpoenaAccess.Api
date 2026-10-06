import {
  Alert,
  Box,
  Card,
  CardContent,
  CircularProgress,
  Typography
} from "@mui/material";

import type { ReportType } from "../../models/ReportType";
import type { CategoryType } from "../../models/CategoryType";

import SubpoenaCountDataRow from "../tables/SubpoenaCountDataRow";
import SubpoenaCountTotalRow from "../tables/SubpoenaCountTotalRow";

export type SubpoenaCountDataRowModel = {
  label: string;
  value: number;
  report: ReportType;
  category: CategoryType;
};

type SubpoenaCountCardProps = {
  title: string;
  loading?: boolean;
  error?: string | null;
  dataRows?: SubpoenaCountDataRowModel[];
  total?: number;
};

export default function SubpoenaCountCard({
  title,
  loading = false,
  error = null,
  dataRows = [],
  total = 0
}: SubpoenaCountCardProps) {

  if (loading) {
    return (
      <Card sx={{ height: "100%", px: 2 }}>
        <CardContent>
          <Typography sx={{ fontWeight: "bold" }} variant="h6">
            {title}
          </Typography>

          <Box
            sx={{
              display: "flex",
              justifyContent: "center",
              py: 3
            }}
          >
            <CircularProgress />
          </Box>
        </CardContent>
      </Card>
    );
  }

  if (error) {
    return (
      <Card sx={{ height: "100%", px: 2 }}>
        <CardContent>
          <Typography sx={{ fontWeight: "bold", mb: 2 }} variant="h6">
            {title}
          </Typography>  
          <Alert severity="error">
            {error}
          </Alert>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card sx={{ height: "100%", px: 2 }}>
      <CardContent>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "flex-start",
            gap: 2,
            mb: 2
          }}
        >
          <Box>
            <Typography sx={{ fontWeight: "bold" }} variant="h6">
              {title}
            </Typography>
          </Box>
        </Box>

        {dataRows.map(dataRow => (
          <SubpoenaCountDataRow
            key={dataRow.label}
            label={dataRow.label}
            value={dataRow.value}
            report={dataRow.report}
            category={dataRow.category}
          />
        ))}

        <SubpoenaCountTotalRow
          label="Total"
          value={total}
        />
      </CardContent>
    </Card>
  );
}