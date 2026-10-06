import TopBanner from "../components/TopBanner";

import cityHall from "../assets/city-hall.jpg";

import {
  Alert,
  AlertTitle,
  Box,
  Paper,
  Button,
  Stack,
  TextField
} from "@mui/material";

import { useState, type SubmitEvent } from "react";
import {
  useLocation,
  useNavigate,
  type Location
} from "react-router-dom";
import { isAxiosError } from "axios";

import {
  login,
  type LoginErrorResponse
} from "../api/authentication";
import { useAuthenticationContext } from "../utils/contextUtils";
import CopyrightFooter from "../components/CopyrightFooter";

type LoginLocationState = {
  from?: Location;
};

export default function Login() {
  const navigate = useNavigate();

  const location = useLocation();

  const { setAuthentication } = useAuthenticationContext();

  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [isLoggingIn, setIsLoggingIn] = useState(false);

  async function submit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();

    setErrorMessage(null);
    setIsLoggingIn(true);

    try {
      const loginResponse = await login({
        userName: userName.trim(),
        password
      });

      setAuthentication(loginResponse.token);

      const loginLocationState = location.state as LoginLocationState | null;

      navigate(loginLocationState?.from ?? "/home", { replace: true });
    }
    catch (error) {
      if (isAxiosError<LoginErrorResponse>(error)) {
        setErrorMessage(
          error.response?.data.detail ??
          "Unable to contact the authentication service. Please try again."
        );
      }
      else {
        setErrorMessage("An unexpected error occurred during login.");
      }
    }
    finally {
      setIsLoggingIn(false);
    }
  }

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
          component="form"
          elevation={8}
          onSubmit={submit}
          sx={{
            p: 5,
            width: 280,
            bgcolor: "rgba(255,255,255,.88)"
          }}
        >
          <Stack spacing={3}>

            {errorMessage &&
              <Alert severity="error">
                <AlertTitle>Login Failed</AlertTitle>
                {errorMessage}
              </Alert>
            }

            <TextField
              label="Username"
              name="username"
              autoComplete="username"
              value={userName}
              onChange={(event) => setUserName(event.target.value)}
              disabled={isLoggingIn}
              autoFocus
              required
              fullWidth
            />

            <TextField
              label="Password"
              name="password"
              type="password"
              autoComplete="current-password"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              disabled={isLoggingIn}
              required
              fullWidth
            />

            <Button
              type="submit"
              variant="contained"
              size="large"
              disabled={isLoggingIn || !userName.trim() || !password}
            >
              {isLoggingIn ? "Logging in..." : "Login"}
            </Button>
          </Stack>
        </Paper>
      </Box>
      <CopyrightFooter/>
    </>
  );
}