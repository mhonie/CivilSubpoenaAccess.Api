import { useState } from "react";
import {
  Alert,
  AlertTitle,
  Box,
  Button,
  CircularProgress,
  Container,
  Grid,
  Link,
  Paper,
  TextField,
  Typography
} from "@mui/material";

import {
  getEvidence,
  updateEvidence
} from "../api/evidence";
import BottomButtons from "../components/BottomButtons";
import TopBanner from "../components/TopBanner";

import { newSubpoenaPdfUrl } from "../utils/pdfUtils";
import CopyrightFooter from "../components/CopyrightFooter";


export default function FixEvidence() {
  const [transactionId, setTransactionId] = useState("");

  const [subpoenaNumber, setSubpoenaNumber] = useState("");

  const [evidence, setEvidence] = useState("");

  const [loadedEvidenceKey, setLoadedEvidenceKey] = useState<string | null>
  (
    null
  );

  const [loading, setLoading] = useState(false);

  const [submitting, setSubmitting] = useState(false);

  const [error, setError] = useState<string | null>(null);

  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  function evidenceKey(): string {
    return `${transactionId.trim()}-${validSubpoenaNumber()}`;
  }

  function validSubpoenaNumber(): number | null {
    const parsedSubpoenaNumber = Number(subpoenaNumber);

    return Number.isInteger(parsedSubpoenaNumber) && parsedSubpoenaNumber > 0

      ? parsedSubpoenaNumber

      : null;
  }

  function changeTransactionId(value: string) {
    setTransactionId(value);
    setLoadedEvidenceKey(null);
    setSuccessMessage(null);
  }

  function changeSubpoenaNumber(value: string) {
    setSubpoenaNumber(value);
    setLoadedEvidenceKey(null);
    setSuccessMessage(null);
  }

  async function loadEvidence() {
    const trimmedTransactionId = transactionId.trim();
    const parsedSubpoenaNumber = validSubpoenaNumber();

    if (!trimmedTransactionId || parsedSubpoenaNumber === null) {
      setError("Enter a valid order number and sequence number.");
      return;
    }

    setLoading(true);
    setError(null);
    setSuccessMessage(null);
    setLoadedEvidenceKey(null);

    try {
      const existingEvidence = await getEvidence(
        trimmedTransactionId,

        parsedSubpoenaNumber
      );

      setEvidence(existingEvidence);

      setLoadedEvidenceKey(
        `${trimmedTransactionId}-${parsedSubpoenaNumber}`
      );
    }
    catch (loadError) {
      setEvidence("");

      setError(
        loadError instanceof Error

          ? loadError.message

          : "Unable to load the evidence."
      );
    }
    finally {
      setLoading(false);
    }
  }

  async function submitEvidence() {
    const trimmedTransactionId = transactionId.trim();
    const parsedSubpoenaNumber = validSubpoenaNumber();

    if
    (
      !trimmedTransactionId ||
      parsedSubpoenaNumber === null ||
      loadedEvidenceKey !== evidenceKey()
    )
    {
      setError("Load the evidence before submitting changes.");
      return;
    }

    setSubmitting(true);
    setError(null);
    setSuccessMessage(null);

    try {
      await updateEvidence(
        trimmedTransactionId,

        parsedSubpoenaNumber,

        evidence
      );

      setSuccessMessage("The evidence was updated.");
    }
    catch (updateError) {
      setError(
        updateError instanceof Error

          ? updateError.message

          : "Unable to update the evidence."
      );
    }
    finally {
      setSubmitting(false);
    }
  }

  const evidenceLoaded = loadedEvidenceKey === evidenceKey();

  const parsedSubpoenaNumber = validSubpoenaNumber();

  return (
    <>
      <TopBanner />

      <Container sx={{mb: 2, px: 2, maxWidth: 1200}}>
        <Typography variant="h4" sx={{mb: 3}}>
          Fix Evidence Text
        </Typography>

        {error &&
          <Alert severity="error" sx={{mb: 3, textAlign: "left"}}>
            <AlertTitle>
              Unable to Complete Request
            </AlertTitle>

            {error}
          </Alert>
        }

        {successMessage &&
          <Alert severity="success" sx={{mb: 3, textAlign: "left"}}>
            {successMessage}
          </Alert>
        }

        <Paper sx={{p: 3, mb: 3}}>
          <Grid container spacing={3} sx={{textAlign: "left"}}>
            <Grid size={{xs: 12, md: 5}}>
              <TextField
                fullWidth
                label="Order Number"
                value={transactionId}
                onChange={(event) => changeTransactionId(event.target.value)}
                disabled={loading || submitting}
              />
            </Grid>

            <Grid size={{xs: 12, md: 3}}>
              <TextField
                fullWidth
                label="Sequence Number"
                type="number"
                value={subpoenaNumber}
                onChange={(event) => changeSubpoenaNumber(event.target.value)}
                disabled={loading || submitting}
                slotProps={{htmlInput: {min: 1}}}
              />
            </Grid>

            <Grid size={{xs: 12, md: 4}}>
              <Button
                fullWidth
                variant="contained"
                size="large"
                onClick={loadEvidence}
                disabled={loading || submitting}
                sx={{height: "100%"}}
              >
                {loading

                  ? <CircularProgress size={24} color="inherit" />

                  : "Get Evidence Text"
                }
              </Button>
            </Grid>

            {evidenceLoaded &&
              <>
                <Grid size={{xs: 12}}>
                  <TextField
                    fullWidth
                    multiline
                    minRows={4}
                    label="Evidence Text"
                    value={evidence}
                    onChange={(event) => setEvidence(event.target.value)}
                    disabled={submitting}
                    slotProps={{htmlInput: {maxLength: 400}}}
                    helperText={
                      `${evidence.length} of 400 characters entered.`
                    }
                  />
                </Grid>

                <Grid size={{xs: 12}}>
                  <Box sx={{display: "flex", gap: 2}}>
                    <Button
                      variant="contained"
                      onClick={submitEvidence}
                      disabled={submitting}
                    >
                      {submitting

                        ? <CircularProgress size={24} color="inherit" />

                        : "Submit"
                      }
                    </Button>

                    {successMessage && parsedSubpoenaNumber !== null &&
                      <Button
                        variant="outlined"
                        component={Link}
                        href={newSubpoenaPdfUrl(
                          transactionId.trim(),

                          parsedSubpoenaNumber,

                          new Date().toISOString()
                        )}
                        target="_blank"
                        rel="noopener noreferrer"
                      >
                        View PDF
                      </Button>
                    }
                  </Box>
                </Grid>
              </>
            }
          </Grid>
        </Paper>

        <BottomButtons />
        <CopyrightFooter/>
      </Container>
    </>
  );
}