import axios from "./axios";

import type { UserRole } from

  "../features/authentication/AuthenticationContext";


export type RoleAssignment = {
  userName: string;
  name: UserRole;
};

export async function getRoles(): Promise<RoleAssignment[]>
{
  const response = await axios.get<RoleAssignment[]>("/roles");

  return response.data;
}

export async function createRole
(
  userName: string,

  roleName: UserRole
): Promise<RoleAssignment>
{
  const response = await axios.post<RoleAssignment>
  (
    "/roles",

    {
      userName,

      roleName
    }
  );

  return response.data;
}

export async function updateRole
(
  userName: string,

  roleName: UserRole
): Promise<void>
{
  await axios.put(
    `/roles/${encodeURIComponent(userName)}`,

    {
      roleName
    }
  );
}

export async function deleteRole(userName: string): Promise<void>
{
  await axios.delete(
    `/roles/${encodeURIComponent(userName)}`
  );
}