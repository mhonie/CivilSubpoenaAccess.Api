import { AppBar, Toolbar, Typography, Box } from "@mui/material";

export default function TopBanner() {
  return (
    <AppBar position="static"
      sx={{ mb: 5, py: 2 }}>
      <Toolbar
        sx={{ justifyContent: "left", gap: 2 }}
      >
        <Box
          component="img"
          src={`${import.meta.env.BASE_URL}philadelphia-logo.png`}
          alt="Philadelphia Logo"
          sx={{ width: 60, height: 60 }}
        />

        <Box sx={{ justifyItems: "left" }}>
          <Typography>
            THE PHILADELPHIA COURTS
          </Typography>
          <Typography variant="h4">
            Civil Subpoena Access
          </Typography>
        </Box>

      </Toolbar>
    </AppBar>
  );
}