import TopBanner from "../components/TopBanner";
import ReportsFilters from "../features/reports/ReportsFilters";
import ReportsCards from "../features/reports/ReportsCards";

import { Container, Typography } from "@mui/material";
import BottomButtons from "../components/BottomButtons";
import CopyrightFooter from "../components/CopyrightFooter";

export default function Reports() {
  return (
    <>
      <TopBanner/>
      <Container
        maxWidth={false}
        sx={{
          width: "100%",
          maxWidth: 1200,
          mx: "auto",
          px: {xs: 2, md: 3}
        }}
      >
        <Typography variant="h4" sx={{mb: 4}}>
          Reports & Transactions
        </Typography>
        <ReportsFilters />
        <ReportsCards />
      </Container>
      <BottomButtons/>
      <CopyrightFooter/>
    </>
  );
}