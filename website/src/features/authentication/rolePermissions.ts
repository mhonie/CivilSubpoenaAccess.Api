import type { UserRole } from "./AuthenticationContext";


export const pendingApprovalRoles: readonly UserRole[] =

  ["Certification", "Admin", "SuperAdmin"];

export const pendingPaymentRoles: readonly UserRole[] =

  ["Finance", "Admin", "SuperAdmin"];

export const approvalRoles: readonly UserRole[] =

  ["Certification", "Admin", "SuperAdmin"];

export const narrativeRoles: readonly UserRole[] =

  ["Certification", "Admin", "SuperAdmin"];

export const fixEvidenceRoles: readonly UserRole[] =

  ["Certification", "Admin", "SuperAdmin"];

export const roleAdministrationRoles: readonly UserRole[] =

  ["SuperAdmin"];

export function rolePermitted
(
  role: UserRole | null,

  permittedRoles: readonly UserRole[]
): boolean
{
  return role !== null && permittedRoles.includes(role);
}