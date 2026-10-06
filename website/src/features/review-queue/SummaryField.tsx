import { Grid, Typography } from "@mui/material";

export default function SummaryField
(
  {
    label,
    value
  }:
  {
    label: string;
    value: string | null | undefined;
  }
)
{
  return (
    <Grid size={{xs: 12, sm: 6, md: 3}}>
      <Typography variant="subtitle2" color="text.secondary">
        {label}
      </Typography>

      <Typography>
        {value || "—"}
      </Typography>
    </Grid>
  );
}