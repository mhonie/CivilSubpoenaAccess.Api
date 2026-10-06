import {
  useEffect,
  useState
} from "react";

import axios from "axios";

import {
  Alert,
  AlertTitle,
  Box,
  Button,
  CircularProgress,
  Container,
  FormControl,
  Grid,
  InputLabel,
  MenuItem,
  Paper,
  Select,
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
  createRole,
  deleteRole,
  getRoles,
  updateRole,

  type RoleAssignment
} from "../api/roles";

import BottomButtons from "../components/BottomButtons";
import CopyrightFooter from "../components/CopyrightFooter";
import TopBanner from "../components/TopBanner";

import type { UserRole } from

  "../features/authentication/AuthenticationContext";


const userRoles: readonly UserRole[] =

  ["Certification", "Finance", "Admin", "SuperAdmin"];

type ProblemDetails = {
  title?: string;
  detail?: string;
};

function ErrorMessage
(
  error: unknown,

  defaultMessage: string
): string
{
  if (!axios.isAxiosError<ProblemDetails>(error))

    return error instanceof Error

      ? error.message

      : defaultMessage;

  return error.response?.data.detail

    ?? error.response?.data.title

    ?? defaultMessage;
}

function IsLdapUserName(userName: string): boolean
{
  return /^[^.]+\.[^.]+$/.test(userName.trim());
}

export default function Administration()
{
  const [roles, setRoles] = useState<RoleAssignment[]>([]);

  const [selectedRoles, setSelectedRoles] =

    useState<Record<string, UserRole>>({});

  const [newUserName, setNewUserName] = useState("");

  const [newRole, setNewRole] =

    useState<UserRole>("Certification");

  const [loading, setLoading] = useState(true);

  const [creating, setCreating] = useState(false);

  const [savingUserName, setSavingUserName] =

    useState<string | null>(null);

  const [deletingUserName, setDeletingUserName] =

    useState<string | null>(null);

  const [error, setError] = useState<string | null>(null);

  const [successMessage, setSuccessMessage] =

    useState<string | null>(null);

  function SetAssignments(roleAssignments: RoleAssignment[])
  {
    const ldapRoleAssignments = roleAssignments.filter(roleAssignment =>

      IsLdapUserName(roleAssignment.userName)
    );

    setRoles(ldapRoleAssignments);

    setSelectedRoles(
      Object.fromEntries(
        ldapRoleAssignments.map(role => [
          role.userName,

          role.name
        ])
      )
    );
  }

  useEffect(() =>
  {
    let cancelled = false;

    async function LoadRoles()
    {
      setLoading(true);

      setError(null);

      try
      {
        const roleAssignments = await getRoles();

        if (!cancelled)

          SetAssignments(roleAssignments);
      }
      catch (loadError)
      {
        if (!cancelled)

          setError(
            ErrorMessage(
              loadError,

              "Unable to load role assignments."
            )
          );
      }
      finally
      {
        if (!cancelled)

          setLoading(false);
      }
    }

    void LoadRoles();

    return () =>
    {
      cancelled = true;
    };
  }, []);

  async function AddRole()
  {
    const userName = newUserName.trim();

    if (!userName)
    {
      setError("Enter a username.");

      return;
    }

    setCreating(true);

    setError(null);

    setSuccessMessage(null);

    try
    {
      const roleAssignment =

        await createRole(userName, newRole);

      SetAssignments(
        [...roles, roleAssignment].sort(
          (left, right) =>

            left.userName.localeCompare(right.userName)
        )
      );

      setNewUserName("");

      setNewRole("Certification");

      setSuccessMessage(
        `The role for ${userName} was created.`
      );
    }
    catch (createError)
    {
      setError(
        ErrorMessage(
          createError,

          "Unable to create the role assignment."
        )
      );
    }
    finally
    {
      setCreating(false);
    }
  }

  async function SaveRole(roleAssignment: RoleAssignment)
  {
    const selectedRole =

      selectedRoles[roleAssignment.userName];

    if (!selectedRole || selectedRole == roleAssignment.name)

      return;

    setSavingUserName(roleAssignment.userName);

    setError(null);

    setSuccessMessage(null);

    try
    {
      await updateRole(
        roleAssignment.userName,

        selectedRole
      );

      setRoles(current => current.map(role =>

        role.userName == roleAssignment.userName

          ? {
              ...role,

              name: selectedRole
            }

          : role
      ));

      setSuccessMessage(
        `The role for ${roleAssignment.userName} was updated.`
      );
    }
    catch (updateError)
    {
      setSelectedRoles(current => ({
        ...current,

        [roleAssignment.userName]: roleAssignment.name
      }));

      setError(
        ErrorMessage(
          updateError,

          "Unable to update the role assignment."
        )
      );
    }
    finally
    {
      setSavingUserName(null);
    }
  }

  async function RemoveRole(roleAssignment: RoleAssignment)
  {
    if (!window.confirm(
      `Delete the role assignment for ${roleAssignment.userName}?`
    ))

      return;

    setDeletingUserName(roleAssignment.userName);

    setError(null);

    setSuccessMessage(null);

    try
    {
      await deleteRole(roleAssignment.userName);

      SetAssignments(
        roles.filter(role =>

          role.userName != roleAssignment.userName
        )
      );

      setSuccessMessage(
        `The role for ${roleAssignment.userName} was deleted.`
      );
    }
    catch (deleteError)
    {
      setError(
        ErrorMessage(
          deleteError,

          "Unable to delete the role assignment."
        )
      );
    }
    finally
    {
      setDeletingUserName(null);
    }
  }

  const mutationInProgress =

    creating

    || savingUserName !== null

    || deletingUserName !== null;

  return (
    <>
      <TopBanner />

      <Container sx={{mb: 2, px: 2, maxWidth: 1200}}>
        <Typography variant="h4" sx={{mb: 3}}>
          Role Administration
        </Typography>

        {error &&
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

        {successMessage &&
          <Alert
            severity="success"
            sx={{mb: 3, textAlign: "left"}}
          >
            {successMessage}
          </Alert>
        }

        <Paper sx={{p: 3, mb: 3}}>
          <Typography
            variant="h6"
            sx={{mb: 3, textAlign: "left"}}
          >
            Add Role Assignment
          </Typography>

          <Grid container spacing={2}>
            <Grid size={{xs: 12, md: 5}}>
              <TextField
                fullWidth
                label="Username"
                value={newUserName}
                disabled={mutationInProgress}
                onChange={(event) =>
                  setNewUserName(event.target.value)
                }
              />
            </Grid>

            <Grid size={{xs: 12, md: 4}}>
              <FormControl fullWidth>
                <InputLabel id="new-user-role-label">
                  Role
                </InputLabel>

                <Select
                  labelId="new-user-role-label"
                  label="Role"
                  value={newRole}
                  disabled={mutationInProgress}
                  onChange={(event) =>
                    setNewRole(event.target.value as UserRole)
                  }
                >
                  {userRoles.map(role =>
                    <MenuItem key={role} value={role}>
                      {role}
                    </MenuItem>
                  )}
                </Select>
              </FormControl>
            </Grid>

            <Grid size={{xs: 12, md: 3}}>
              <Button
                fullWidth
                type="button"
                variant="contained"
                size="large"
                disabled={
                  mutationInProgress

                  || !newUserName.trim()
                }
                onClick={() => void AddRole()}
                sx={{height: "100%"}}
              >
                {creating

                  ? (
                      <CircularProgress
                        size={20}
                        color="inherit"
                      />
                    )

                  : "Add"
                }
              </Button>
            </Grid>
          </Grid>
        </Paper>

        <TableContainer component={Paper} sx={{mb: 3}}>
          <Table aria-label="Role assignments">
            <TableHead>
              <TableRow>
                <TableCell>
                  Username
                </TableCell>

                <TableCell>
                  Role
                </TableCell>

                <TableCell align="right">
                  Actions
                </TableCell>
              </TableRow>
            </TableHead>

            <TableBody>
              {loading &&
                <TableRow>
                  <TableCell colSpan={3} align="center">
                    <CircularProgress
                      size={24}
                      sx={{mr: 2}}
                    />

                    Loading role assignments...
                  </TableCell>
                </TableRow>
              }

              {!loading && roles.length == 0 &&
                <TableRow>
                  <TableCell colSpan={3} align="center">
                    No role assignments were found.
                  </TableCell>
                </TableRow>
              }

              {!loading && roles.map(roleAssignment =>
                <TableRow key={roleAssignment.userName}>
                  <TableCell>
                    {roleAssignment.userName}
                  </TableCell>

                  <TableCell sx={{width: 300}}>
                    <FormControl fullWidth size="small">
                      <Select
                        value={
                          selectedRoles[roleAssignment.userName]

                          ?? roleAssignment.name
                        }
                        disabled={mutationInProgress}
                        onChange={(event) =>
                          setSelectedRoles(current => ({
                            ...current,

                            [roleAssignment.userName]:

                              event.target.value as UserRole
                          }))
                        }
                      >
                        {userRoles.map(role =>
                          <MenuItem key={role} value={role}>
                            {role}
                          </MenuItem>
                        )}
                      </Select>
                    </FormControl>
                  </TableCell>

                  <TableCell align="right">
                    <Box
                      sx={{
                        display: "flex",

                        justifyContent: "flex-end",

                        gap: 1
                      }}
                    >
                      <Button
                        type="button"
                        variant="contained"
                        size="small"
                        disabled={
                          mutationInProgress

                          || selectedRoles[
                            roleAssignment.userName
                          ] == roleAssignment.name
                        }
                        onClick={() =>
                          void SaveRole(roleAssignment)
                        }
                      >
                        {savingUserName == roleAssignment.userName

                          ? (
                              <CircularProgress
                                size={18}
                                color="inherit"
                              />
                            )

                          : "Save"
                        }
                      </Button>

                      <Button
                        type="button"
                        variant="outlined"
                        color="error"
                        size="small"
                        disabled={mutationInProgress}
                        onClick={() =>
                          void RemoveRole(roleAssignment)
                        }
                      >
                        {deletingUserName == roleAssignment.userName

                          ? (
                              <CircularProgress
                                size={18}
                                color="inherit"
                              />
                            )

                          : "Delete"
                        }
                      </Button>
                    </Box>
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>

        <BottomButtons />
        <CopyrightFooter />
      </Container>
    </>
  );
}