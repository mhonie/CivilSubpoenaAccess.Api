import { Grid } from "@mui/material";

import SubpoenaCountByStatusCard from "./SubpoenaCountByStatusCard";
import SubpoenaCountByTypeCard from "./SubpoenaCountByTypeCard";
import SubpoenaCountByUserTypeCard from "./SubpoenaCountByUserTypeCard";
import PaymentsByTypeCard from "./PaymentsByTypeCard";

export default function ReportsCards() {
    return (
    <>
      <Grid container spacing={2} sx={{mb: 5}} >
        <Grid size={{ xs: 12, sm: 6, lg: 4 }}>
          <SubpoenaCountByStatusCard />
        </Grid>

        <Grid size={{ xs: 12, sm: 6, lg: 4 }}>
          <SubpoenaCountByTypeCard />
        </Grid>

        <Grid size={{ xs: 12, sm: 6, lg: 4 }}>
          <SubpoenaCountByUserTypeCard />
        </Grid>
      </Grid>

      <Grid container spacing={2}>
        <Grid size={12} sx={{mb: 5}}>
          <PaymentsByTypeCard />
        </Grid>
      </Grid>
    </>
    );
}