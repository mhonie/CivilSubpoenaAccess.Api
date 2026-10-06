import {
  Navigate,
  Outlet,
  useLocation
} from "react-router-dom";

import type { UserRole } from

  "../features/authentication/AuthenticationContext";

import { rolePermitted } from

  "../features/authentication/rolePermissions";

import { useAuthenticationContext } from "../utils/contextUtils";


type ProtectedRouteProps = {
  allowedRoles?: readonly UserRole[];
};

export default function ProtectedRoute
(
  {
    allowedRoles
  }: ProtectedRouteProps
)
{
  const {
    isAuthenticated,
    role
  } = useAuthenticationContext();

  const location = useLocation();

  if (!isAuthenticated)

    return (
      <Navigate
        to="/"
        state={{from: location}}
        replace
      />
    );

  if (allowedRoles && !rolePermitted(role, allowedRoles))

    return <Navigate to="/home" replace />;

  return <Outlet />;
}