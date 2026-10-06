import TopBanner from "../components/TopBanner";

import cityHall from "../assets/city-hall.jpg";

import {
  Box,
  Paper,
  Button,
  Stack
} from "@mui/material";

import { Link } from "react-router-dom";

import { useAuthenticationContext } from "../utils/contextUtils";
import CopyrightFooter from "../components/CopyrightFooter";


export default function Home() {
  const { clearAuthentication } = useAuthenticationContext();

  return (
    <>
      <TopBanner/>
      <Box
        sx={{
          minHeight: "calc(100vh - 128px)",
          backgroundImage: `url(${cityHall})`,
          backgroundSize: "cover",
          backgroundPosition: "center",
          display: "flex",
          justifyContent: "right",
          alignItems: "right",
          p: 4,
          mb: 2
        }}
      >
        <Paper
          elevation={8}
          sx={{
            p: 5,
            maxWidth: 500,
            bgcolor: "rgba(255,255,255,.88)",
            display: "flex",
            flexDirection: "column"
          }}
        >
          <Stack
            spacing={3}
            sx={{
              alignItems: "center",
              flexGrow: 1
            }}
          >
            <Stack sx={{flexGrow: 1}}>
              <Button
                variant="contained"
                size="large"
                component={Link}
                to="/queue"
                sx={{mb:2}}
              >
                Search & Review Queue
              </Button>
              <Button
                variant="contained"
                size="large"
                component={Link}
                to="/reports"
                sx={{mb:2}}
              >
                Reports & Transactions
              </Button>
              <Button
                variant="contained"
                size="large"
                component={Link}
                to="/charts"
                sx={{mb:2}}
              >
                Charts & Graphs
              </Button>
              <Button
                variant="contained"
                size="large"
                component={Link}
                to="/fix-evidence"
                sx={{mb:2}}
              >
                Fix Evidence
              </Button>
              <Button
                variant="contained"
                size="large"
                component={Link}
                to="/admin"
                sx={{mb:2}}
              >
                Administration
              </Button>
            </Stack>
            <Button
              variant="contained"
              color="error"
              size="large"
              component={Link}
              to="/"
              onClick={clearAuthentication}
            >
              Log Out
            </Button>
          </Stack>
        </Paper>
      </Box>
      <CopyrightFooter/>
    </>
  );
}