import { useReportsContext } from "../../utils/contextUtils";

import { Grid, TextField } from "@mui/material";


export default function ReportsFilters() {

    const {
        filingStartDate,
        filingEndDate,
        approvedStartDate,
        approvedEndDate,
        setFilingStartDate,
        setFilingEndDate,
        setApprovedStartDate,
        setApprovedEndDate
    } = useReportsContext();

    return (
        <Grid container spacing={2} sx={{mb:5}}>
          <Grid size={{ xs: 12, md: 3 }}>
            <TextField
              fullWidth
              label="Filing Start Date"
              type="date"
              value={filingStartDate}
              onChange={(e) => setFilingStartDate(e.target.value)}
              slotProps={{
                inputLabel: {
                  shrink: true
                }
              }}
            />
          </Grid>

          <Grid size={{ xs: 12, md: 3 }}>
            <TextField
              fullWidth
              label="Filing End Date"
              type="date"
              value={filingEndDate}
              onChange={(e) => setFilingEndDate(e.target.value)}
              slotProps={{
                inputLabel: {
                  shrink: true
                }
              }}
            />
          </Grid>

          <Grid size={{ xs: 12, md: 3 }}>
            <TextField
              fullWidth
              label="Review Start Date"
              type="date"
              value={approvedStartDate}
              onChange={(e) => setApprovedStartDate(e.target.value)}
              slotProps={{
                inputLabel: {
                  shrink: true
                }
              }}
            />
          </Grid>

          <Grid size={{ xs: 12, md: 3 }}>
            <TextField
              fullWidth
              label="Review End Date"
              type="date"
              value={approvedEndDate}
              onChange={(e) => setApprovedEndDate(e.target.value)}
              slotProps={{
                inputLabel: {
                  shrink: true
                }
              }}
            />
          </Grid>
        </Grid>
    );
}