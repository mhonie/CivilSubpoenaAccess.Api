import {
  useEffect,
  useState
} from "react";

import { useParams } from "react-router-dom";

import {
  Alert,
  AlertTitle,
  Box,
  Button,
  CircularProgress,
  Grid,
  Link,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Typography
} from "@mui/material";

import {
  updateApprovals,

  type SubpoenaApproval
} from "../../api/approval";

import {
  useAuthenticationContext,
  useReviewQueueContext
} from "../../utils/contextUtils";

import {
  getNarrative,
  updateNarrative
} from "../../api/narrative";

import {
  getSubpoenaOrderDetail
} from "../../api/subpoenaOrderDetail";

import BottomButtons from "../../components/BottomButtons";
import TopBanner from "../../components/TopBanner";

import type {
  SubpoenaOrderDetail
} from "../../models/SubpoenaOrderDetail";

import { formatDate } from "../../utils/dateUtils";
import { formatCurrency } from "../../utils/mathUtils";
import { newSubpoenaPdfUrl } from "../../utils/pdfUtils";

import {
  approvalRoles,
  narrativeRoles,
  rolePermitted
} from "../authentication/rolePermissions";

import SummaryField from "./SummaryField";


type SubpoenaAction = "Approved" | "Rejected";

export default function SubpoenaOrderDetails()
{
  const {
    transactionId
  } = useParams<{ transactionId: string }>();

  const {
    role
  } = useAuthenticationContext();

  const {
    updateCachedTransaction
  } = useReviewQueueContext();

  const canUpdateApprovals = rolePermitted(
    role,

    approvalRoles
  );

  const canUpdateNarrative = rolePermitted(
    role,

    narrativeRoles
  );

  const [subpoenaOrderDetail, setSubpoenaOrderDetail] =

    useState<SubpoenaOrderDetail | null>(null);

  const orderDecisionComplete = subpoenaOrderDetail !== null

    && subpoenaOrderDetail.subpoenas.length > 0

    && subpoenaOrderDetail.subpoenas.every(subpoena =>

      subpoena.status == "Approved"

      || subpoena.status == "Rejected"
    );

  const [loading, setLoading] = useState(true);

  const [submittingApprovals, setSubmittingApprovals] =

    useState(false);

  const [submittingNarrative, setSubmittingNarrative] =

    useState(false);

  const [error, setError] = useState<string | null>(null);

  const [successMessage, setSuccessMessage] =

    useState<string | null>(null);

  const [messageToFiler, setMessageToFiler] = useState("");

  const [subpoenaActions, setSubpoenaActions] =

    useState<Record<number, SubpoenaAction>>({});

  useEffect(() =>
  {
    let cancelled = false;

    async function Load()
    {
      if (!transactionId)
      {
        setError("A transaction number is required.");

        setLoading(false);

        return;
      }

      setLoading(true);

      setError(null);

      setSuccessMessage(null);

      try
      {
        const orderDetail =

          await getSubpoenaOrderDetail(transactionId);

        const narrative = canUpdateNarrative

          ? await getNarrative(transactionId)

          : "";

        if (!cancelled)
        {
          setSubpoenaOrderDetail(orderDetail);

          setMessageToFiler(narrative);

          setSubpoenaActions({});
        }
      }
      catch (loadError)
      {
        if (!cancelled)
        {
          setSubpoenaOrderDetail(null);

          setError(
            loadError instanceof Error

              ? loadError.message

              : "Unable to load the subpoena order."
          );
        }
      }
      finally
      {
        if (!cancelled)

          setLoading(false);
      }
    }

    void Load();

    return () =>
    {
      cancelled = true;
    };
  }, [transactionId, canUpdateNarrative]);

  async function SubmitApprovals()
  {
    if (!transactionId || !canUpdateApprovals)

      return;

    const approvals: SubpoenaApproval[] =

      Object.entries(subpoenaActions)

        .map(([subpoenaNumber, action]) => ({
          subpoenaNumber: Number(subpoenaNumber),

          approvalIndicator: action == "Approved" ? "A" : "R"
        }));

    if (approvals.length == 0)
    {
      setSuccessMessage(null);

      setError("Select at least one approval or rejection.");

      return;
    }

    if (!window.confirm(
      "Are you sure you want to submit these approval decisions?"
    ))
    
      return;

    setSubmittingApprovals(true);

    setError(null);

    setSuccessMessage(null);

    try
    {
      await updateApprovals(transactionId, approvals);

      const orderDetail =

        await getSubpoenaOrderDetail(transactionId);

      setSubpoenaOrderDetail(orderDetail);

      updateCachedTransaction(orderDetail);

      setSubpoenaActions({});

      setSuccessMessage("The approval decisions were saved.");
    }
    catch (approvalError)
    {
      setError(
        approvalError instanceof Error

          ? approvalError.message

          : "Unable to save the approval decisions."
      );
    }
    finally
    {
      setSubmittingApprovals(false);
    }
  }

  async function SubmitNarrative()
  {
    if (!transactionId || !canUpdateNarrative)

      return;

    setSubmittingNarrative(true);

    setError(null);

    setSuccessMessage(null);

    try
    {
      await updateNarrative(transactionId, messageToFiler);

      setSuccessMessage("The message to the filer was saved.");
    }
    catch (narrativeError)
    {
      setError(
        narrativeError instanceof Error

          ? narrativeError.message

          : "Unable to save the message to the filer."
      );
    }
    finally
    {
      setSubmittingNarrative(false);
    }
  }

  return (
    <>
      <TopBanner />

      <Box sx={{mb: 2, px: 2}}>
        <Typography variant="h4">
          Order Details
        </Typography>

        {loading &&
          <Box
            sx={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: 2,
              py: 6
            }}
          >
            <CircularProgress size={28} />

            <Typography>
              Loading subpoena order...
            </Typography>
          </Box>
        }

        {!loading && error &&
          <Alert
            severity="error"
            sx={{mb: 3, textAlign: "left"}}
          >
            <AlertTitle>
              Unable to Complete Request
            </AlertTitle>

            {error}
          </Alert>
        }

        {!loading && successMessage &&
          <Alert
            severity="success"
            sx={{mb: 3, textAlign: "left"}}
          >
            {successMessage}
          </Alert>
        }

        {!loading && subpoenaOrderDetail &&
          <>
            <Paper sx={{p: 3, mb: 3}}>
              <Typography
                variant="h6"
                sx={{mb: 3, textAlign: "left"}}
              >
                Summary
              </Typography>

              <Grid
                container
                spacing={3}
                sx={{textAlign: "left"}}
              >
                <SummaryField
                  label="Order #"
                  value={subpoenaOrderDetail.transactionId}
                />

                <SummaryField
                  label="Status"
                  value={subpoenaOrderDetail.status}
                />

                <SummaryField
                  label="Payment Type"
                  value={subpoenaOrderDetail.paymentType}
                />

                <SummaryField
                  label="Date Submitted"
                  value={
                    formatDate(subpoenaOrderDetail.dateSubmitted)
                  }
                />

                <SummaryField
                  label="Username"
                  value={subpoenaOrderDetail.username}
                />

                <SummaryField
                  label="Email Address"
                  value={subpoenaOrderDetail.email}
                />

                <SummaryField
                  label="Clerk"
                  value={subpoenaOrderDetail.reviewClerk}
                />

                <SummaryField
                  label="Action Date"
                  value={
                    formatDate(subpoenaOrderDetail.actionDate)
                  }
                />
              </Grid>
            </Paper>

            <TableContainer component={Paper} sx={{mb: 3}}>
              <Table aria-label="Subpoenas in this order">
                <TableHead>
                  <TableRow>
                    <TableCell>
                      Subpoena #
                    </TableCell>

                    <TableCell>
                      Caption
                    </TableCell>

                    <TableCell>
                      Case ID
                    </TableCell>

                    <TableCell>
                      Date Filed
                    </TableCell>

                    <TableCell align="right">
                      Fee
                    </TableCell>

                    <TableCell>
                      Status
                    </TableCell>

                    <TableCell>
                      Document
                    </TableCell>

                    {canUpdateApprovals &&
                      <TableCell>
                        Action
                      </TableCell>
                    }
                  </TableRow>
                </TableHead>

                <TableBody>
                  {subpoenaOrderDetail.subpoenas.map(subpoena =>
                    <TableRow key={subpoena.subpoenaNumber}>
                      <TableCell>
                        {subpoena.subpoenaNumber}
                      </TableCell>

                      <TableCell>
                        {subpoena.caption || "—"}
                      </TableCell>

                      <TableCell>
                        {subpoena.caseId || "—"}
                      </TableCell>

                      <TableCell>
                        {formatDate(subpoena.filingDate) || "—"}
                      </TableCell>

                      <TableCell align="right">
                        {formatCurrency(subpoena.fee)}
                      </TableCell>

                      <TableCell>
                        {subpoena.status || "—"}
                      </TableCell>

                      <TableCell>
                        {subpoena.subpoenaType
                          ?
                            <Link
                              href={
                                newSubpoenaPdfUrl(
                                  subpoenaOrderDetail.transactionId,

                                  subpoena.subpoenaNumber,

                                  subpoenaOrderDetail.actionDate
                                )
                              }
                              target="_blank"
                              rel="noopener noreferrer"
                            >
                              {subpoena.subpoenaType}
                            </Link>
                          : "—"
                        }
                      </TableCell>

                      {canUpdateApprovals &&
                        <TableCell>
                          <Box
                            sx={{
                              display: "flex",

                              gap: 1
                            }}
                          >
                            <Button
                              type="button"
                              size="small"
                              variant={
                                subpoenaActions[
                                  subpoena.subpoenaNumber
                                ] == "Approved"

                                  ? "contained"

                                  : "outlined"
                              }
                              disabled={
                                subpoena.status == "Approved"

                                || submittingApprovals
                              }
                              onClick={() =>
                                setSubpoenaActions(actions => ({
                                  ...actions,

                                  [subpoena.subpoenaNumber]:
                                    "Approved"
                                }))
                              }
                            >
                              Approve
                            </Button>

                            <Button
                              type="button"
                              size="small"
                              color="error"
                              variant={
                                subpoenaActions[
                                  subpoena.subpoenaNumber
                                ] == "Rejected"

                                  ? "contained"

                                  : "outlined"
                              }
                              disabled={
                                subpoena.status == "Rejected"

                                || submittingApprovals
                              }
                              onClick={() =>
                                setSubpoenaActions(actions => ({
                                  ...actions,

                                  [subpoena.subpoenaNumber]:
                                    "Rejected"
                                }))
                              }
                            >
                              Reject
                            </Button>
                          </Box>
                        </TableCell>
                      }
                    </TableRow>
                  )}

                  {subpoenaOrderDetail.convenienceFee > 0 &&
                    <TableRow>
                      <TableCell colSpan={4} align="right">
                        <Typography >
                          Convenience Fee
                        </Typography>
                      </TableCell>

                      <TableCell align="right">
                        <Typography >
                          {formatCurrency(
                            subpoenaOrderDetail.convenienceFee
                          )}
                        </Typography>
                      </TableCell>

                      <TableCell
                        colSpan={canUpdateApprovals ? 3 : 2}
                      />
                    </TableRow>
                  }

                  <TableRow>
                    <TableCell colSpan={4} align="right">
                      <Typography sx={{fontWeight: "bold"}}>
                        Total
                      </Typography>
                    </TableCell>

                    <TableCell align="right">
                      <Typography sx={{fontWeight: "bold"}}>
                        {formatCurrency(
                          subpoenaOrderDetail.totalFee
                        )}
                      </Typography>
                    </TableCell>

                    <TableCell
                      colSpan={canUpdateApprovals ? 3 : 2}
                    />
                  </TableRow>
                </TableBody>
              </Table>

              {canUpdateApprovals &&
                <Box
                  sx={{
                    display: "flex",

                    justifyContent: "flex-end",

                    p: 2
                  }}
                >
                  <Button
                    type="button"
                    variant="contained"
                    disabled={
                      Object.keys(subpoenaActions).length == 0

                      || submittingApprovals
                    }
                    onClick={() => void SubmitApprovals()}
                  >
                    {submittingApprovals

                      ? (
                          <CircularProgress
                            size={20}
                            color="inherit"
                          />
                        )

                      : "Submit"
                    }
                  </Button>
                </Box>
              }
            </TableContainer>

            {canUpdateNarrative &&
              <Paper sx={{p: 3, mb: 3}}>
                <TextField
                  fullWidth
                  multiline
                  minRows={4}
                  label="Message To Filer"
                  value={messageToFiler}
                  disabled={submittingNarrative || orderDecisionComplete}
                  onChange={(event) =>
                    setMessageToFiler(event.target.value)
                  }
                  slotProps={{
                    htmlInput: {
                      maxLength: 400
                    }
                  }}
                  helperText={`${messageToFiler.length}/400`}
                  sx={{textAlign: "left"}}
                />

                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "flex-end",
                    mt: 2
                  }}
                >
                  <Button
                    type="button"
                    variant="contained"
                    disabled={submittingNarrative || orderDecisionComplete}
                    onClick={() => void SubmitNarrative()}
                  >
                    {submittingNarrative

                      ? (
                          <CircularProgress
                            size={20}
                            color="inherit"
                          />
                        )

                      : "Submit"
                    }
                  </Button>
                </Box>
              </Paper>
            }

            <BottomButtons backTo="/queue" />
          </>
        }
      </Box>
    </>
  );
}