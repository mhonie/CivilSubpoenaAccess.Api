import { Box, Typography } from "@mui/material";
import { formatCount } from "../../utils/mathUtils";

type SubpoenaCountTotalRowProps = {
  label: string;
  value: number;
};

export default function SubpoenaCountTotalRow({
  label,
  value
}: SubpoenaCountTotalRowProps) {
  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "space-between",
        mt: 3
      }}
    >
      <Typography sx={{fontWeight: "bold"}}>{label}</Typography>

      <Typography sx={{fontWeight: "bold"}}>
        {formatCount(value)}
      </Typography>
    </Box>
  );
}