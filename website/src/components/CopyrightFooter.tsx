import { Typography, Box } from "@mui/material";

export default function TopBanner() {
  return (
    <Box
    component="footer"
    sx={{
      py: 2,
      textAlign: "center"
    }}
  >
    <Typography variant="body2" sx={{mb: 2}}>
      © First Judicial District of Pennsylvania. All rights reserved.
    </Typography>
  </Box>
  );
}