import {
  Box,
  Button,
  Checkbox,
  FormControl,
  FormControlLabel,
  FormLabel,
  Grid,
  Radio,
  RadioGroup,
  TextField
} from "@mui/material";

import type {
  ReviewQueueFilters as FilterValues
} from "../../models/ReviewQueueFilters";

import { useAuthenticationContext } from "../../utils/contextUtils";

import {
  pendingApprovalRoles,
  pendingPaymentRoles,
  rolePermitted
} from "../authentication/rolePermissions";


type StatusFilter =

  "All"

  | "Approved"

  | "Rejected"

  | "Pending Approval"

  | "Pending Payment";

type ReviewQueueFiltersProps = {
  filters: FilterValues;
  loading: boolean;
  onChange: (filters: FilterValues) => void;
  onApply: () => void;
  onReset: () => void;
};

function SelectedStatus(filters: FilterValues): StatusFilter
{
  if (filters.isPendingApproval)

    return "Pending Approval";

  if (filters.isPendingPayment)

    return "Pending Payment";

  if (filters.status == "Approved")

    return "Approved";

  if (filters.status == "Rejected")

    return "Rejected";

  return "All";
}

function WithStatus
(
  filters: FilterValues,

  status: StatusFilter
): FilterValues
{
  return {
    ...filters,

    status:

      status == "Pending Approval"

      || status == "Pending Payment"

        ? null

        : status,

    isPendingApproval: status == "Pending Approval",

    isPendingPayment: status == "Pending Payment"
  };
}

export default function ReviewQueueFilters
(
  {
    filters,
    loading,
    onChange,
    onApply,
    onReset
  }: ReviewQueueFiltersProps
)
{
  const {
    role
  } = useAuthenticationContext();

  const canViewPendingApproval = rolePermitted(
    role,
    pendingApprovalRoles
  );

  const canViewPendingPayment = rolePermitted(
    role,
    pendingPaymentRoles
  );

  return (
    <Box sx={{mb: 4, textAlign: "left"}}>
      <FormControl sx={{mb: 3}}>
        <FormLabel>
          Status
        </FormLabel>

        <RadioGroup
          row
          value={SelectedStatus(filters)}
          onChange={(event) =>
            onChange(
              WithStatus(
                filters,

                event.target.value as StatusFilter
              )
            )
          }
          sx={{mb: 1}}
        >
          {canViewPendingApproval &&
            <FormControlLabel
              value="Pending Approval"
              control={<Radio />}
              label="Pending Approval"
            />
          }

          <FormControlLabel
            value="Approved"
            control={<Radio />}
            label="Approved"
          />

          {canViewPendingPayment &&
            <FormControlLabel
              value="Pending Payment"
              control={<Radio />}
              label="Pending Payment"
            />
          }

          <FormControlLabel
            value="Rejected"
            control={<Radio />}
            label="Rejected"
          />

          <FormControlLabel
            value="All"
            control={<Radio />}
            label="All"
          />
        </RadioGroup>

        <FormControlLabel
          control={
            <Checkbox
              checked={filters.includeWebPayments}
              onChange={(event) =>
                onChange({
                  ...filters,

                  includeWebPayments: event.target.checked
                })
              }
            />
          }
          label="Include Web Payments"
        />
      </FormControl>

      <Grid container spacing={2} sx={{mb: 2}}>
        <Grid size={{xs: 12, md: 3}}>
          <TextField
            fullWidth
            label="Transaction #"
            value={filters.transactionNumber}
            onChange={(event) =>
              onChange({
                ...filters,

                transactionNumber: event.target.value
              })
            }
          />
        </Grid>

        <Grid size={{xs: 12, md: 3}}>
          <TextField
            fullWidth
            label="Case ID"
            value={filters.caseId}
            onChange={(event) =>
              onChange({
                ...filters,

                caseId: event.target.value
              })
            }
          />
        </Grid>

        <Grid size={{xs: 12, md: 3}}>
          <TextField
            fullWidth
            label="Submitter Email"
            value={filters.submitterEmail}
            onChange={(event) =>
              onChange({
                ...filters,

                submitterEmail: event.target.value
              })
            }
          />
        </Grid>

        <Grid size={{xs: 12, md: 3}}>
          <TextField
            fullWidth
            label="Review Clerk"
            value={filters.reviewClerk}
            onChange={(event) =>
              onChange({
                ...filters,

                reviewClerk: event.target.value
              })
            }
          />
        </Grid>
      </Grid>

      <Grid container spacing={2} sx={{mb: 4}}>
        <Grid size={{xs: 12, sm: 6, md: 3}}>
          <TextField
            fullWidth
            label="Filing Start Date"
            type="date"
            value={filters.filingStartDate}
            onChange={(event) =>
              onChange({
                ...filters,

                filingStartDate: event.target.value
              })
            }
            slotProps={{
              inputLabel: {
                shrink: true
              }
            }}
          />
        </Grid>

        <Grid size={{xs: 12, sm: 6, md: 3}}>
          <TextField
            fullWidth
            label="Filing End Date"
            type="date"
            value={filters.filingEndDate}
            onChange={(event) =>
              onChange({
                ...filters,

                filingEndDate: event.target.value
              })
            }
            slotProps={{
              inputLabel: {
                shrink: true
              }
            }}
          />
        </Grid>

        <Grid size={{xs: 12, sm: 6, md: 3}}>
          <TextField
            fullWidth
            label="Review Start Date"
            type="date"
            value={filters.reviewStartDate}
            onChange={(event) =>
              onChange({
                ...filters,

                reviewStartDate: event.target.value
              })
            }
            slotProps={{
              inputLabel: {
                shrink: true
              }
            }}
          />
        </Grid>

        <Grid size={{xs: 12, sm: 6, md: 3}}>
          <TextField
            fullWidth
            label="Review End Date"
            type="date"
            value={filters.reviewEndDate}
            onChange={(event) =>
              onChange({
                ...filters,

                reviewEndDate: event.target.value
              })
            }
            slotProps={{
              inputLabel: {
                shrink: true
              }
            }}
          />
        </Grid>
      </Grid>

      <Grid container spacing={2}>
        <Grid
          size={{xs: 12, md: 4}}
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "flex-start",
            gap: 2
          }}
        >
          <Button
            type="button"
            variant="contained"
            disabled={loading}
            onClick={onApply}
          >
            Apply Filters
          </Button>

          <Button
            type="button"
            variant="outlined"
            color="error"
            disabled={loading}
            onClick={onReset}
          >
            Reset
          </Button>
        </Grid>
      </Grid>
    </Box>
  );
}