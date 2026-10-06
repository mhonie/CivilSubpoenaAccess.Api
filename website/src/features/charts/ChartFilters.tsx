import { Box, Grid, TextField } from "@mui/material";


type ChartFiltersProps = {
  filingStartDate: string;
  filingEndDate: string;
  disabled?: boolean;
  onFilingStartDateChange?: (value: string) => void;
  onFilingEndDateChange?: (value: string) => void;
};

export default function ChartFilters
(
  {
    filingStartDate,

    filingEndDate,

    disabled = false,

    onFilingStartDateChange,

    onFilingEndDateChange
  }: ChartFiltersProps
)
{
  return (
    <Box
      sx={{
        width: "100%",

        maxWidth: 700,

        mx: "auto"
      }}
    >
      <Grid container spacing={2} sx={{mb: 5}}>
        <Grid size={{xs: 12, md: 6}}>
          <TextField
            fullWidth
            disabled={disabled}
            label="Filing Start Date"
            type="date"
            value={filingStartDate}
            onChange={(event) =>
              onFilingStartDateChange?.(event.target.value)
            }
            slotProps={{
              inputLabel: {
                shrink: true
              }
            }}
          />
        </Grid>

        <Grid size={{xs: 12, md: 6}}>
          <TextField
            fullWidth
            disabled={disabled}
            label="Filing End Date"
            type="date"
            value={filingEndDate}
            onChange={(event) =>
              onFilingEndDateChange?.(event.target.value)
            }
            slotProps={{
              inputLabel: {
                shrink: true
              }
            }}
          />
        </Grid>
      </Grid>
    </Box>
  );
}