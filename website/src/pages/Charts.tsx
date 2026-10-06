import {
  Box,
  Grid,
  Typography
} from "@mui/material";

import BottomButtons from "../components/BottomButtons";
import CopyrightFooter from "../components/CopyrightFooter";
import TopBanner from "../components/TopBanner";

import SubpoenaCharts from "../features/charts/SubpoenaCharts";

import { useChartContext } from "../utils/contextUtils";


export default function Charts()
{
  const {
    filingStartDate,
    filingEndDate,
    previousYearFilingStartDate,
    previousYearFilingEndDate,
    dateError,
    setFilingStartDate,
    setFilingEndDate
  } = useChartContext();

  return (
    <>
      <TopBanner />

      <Typography variant="h4" sx={{mb: 4}}>
        Charts & Graphs
      </Typography>

      <Box sx={{px: 2}}>
        <Grid container spacing={4} >
          <Grid size={{xs: 12, md: 6}}>
            <SubpoenaCharts
              title="Year Prior"
              filingStartDate={previousYearFilingStartDate}
              filingEndDate={previousYearFilingEndDate}
              dateError={dateError}
              filtersDisabled
            />
          </Grid>

          <Grid size={{xs: 12, md: 6}}>
            <SubpoenaCharts
              title="Current Year"
              filingStartDate={filingStartDate}
              filingEndDate={filingEndDate}
              dateError={dateError}
              onFilingStartDateChange={setFilingStartDate}
              onFilingEndDateChange={setFilingEndDate}
            />
          </Grid>
        </Grid>
      </Box>

      <BottomButtons />

      <CopyrightFooter />
    </>
  );
}