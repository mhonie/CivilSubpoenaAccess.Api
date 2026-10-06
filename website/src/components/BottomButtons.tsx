import { Grid, Button, Box } from "@mui/material";
import { Link, useNavigate } from "react-router-dom";

import { useAuthenticationContext } from "../utils/contextUtils";

type BottomButtonsProps = {
  backTo?: string;
};

export default function BottomButtons
(
  {
    backTo = "/home"
  }: BottomButtonsProps
)
{
  const navigate = useNavigate();

  const { clearAuthentication } = useAuthenticationContext();

  function logout() {
    clearAuthentication();

    navigate("/", {replace: true});
  }

  return (
    <Box
      sx={{
        width: "100%",
        maxWidth: 1200,
        mx: "auto"
      }}
    >
      <Grid container spacing={2} sx={{my: 5}}>
        <Grid size={{xs: 12, md: 6}}>
          <Button
            fullWidth
            variant="outlined"
            color="error"
            size="large"
            component={Link}
            to={backTo}
            sx={{mb: 5}}
          >
            Back
          </Button>
        </Grid>

        <Grid size={{xs: 12, md: 6}}>
          <Button
            fullWidth
            variant="contained"
            color="error"
            size="large"
            onClick={logout}
            sx={{mb: 5}}
          >
            Log Out
          </Button>
        </Grid>
      </Grid>
    </Box>
  );
}