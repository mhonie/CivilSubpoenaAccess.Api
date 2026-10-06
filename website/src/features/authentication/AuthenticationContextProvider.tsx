import {
  useCallback,
  useEffect,
  useState,
  type ReactNode
} from "react";

import {
  AuthenticationContext,
  type UserRole
} from "./AuthenticationContext";

import {
  removeStoredWebToken,
  storedWebToken,
  storeWebToken
} from "./authenticationStorage";


type AuthenticationState = {
  webToken: string;
  userName: string;
  role: UserRole;
  expiresAt: number;
};

type WebTokenPayload = {
  sub?: string;
  exp?: number;
  role?: unknown;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"?: unknown;
};

const maximumTimeoutMilliseconds = 2_147_483_647;

function userRole(value: unknown): value is UserRole
{
  return value == "Certification"

    || value == "Finance"

    || value == "Admin"

    || value == "SuperAdmin";
}

function webTokenPayload(webToken: string): WebTokenPayload
{
  const tokenParts = webToken.split(".");

  if (tokenParts.length != 3)

    throw new Error("The authentication token is invalid.");

  const tokenPayload = tokenParts[1]
    .replaceAll("-", "+")
    .replaceAll("_", "/")
    .padEnd(Math.ceil(tokenParts[1].length / 4) * 4, "=");

  return JSON.parse(atob(tokenPayload)) as WebTokenPayload;
}

function createAuthenticationState
(
  webToken: string
): AuthenticationState
{
  const tokenPayload = webTokenPayload(webToken);

  const role = tokenPayload.role

    ?? tokenPayload[
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ];

  if (
    !tokenPayload.sub

    || typeof tokenPayload.exp != "number"

    || !userRole(role)
  )

    throw new Error("The authentication token is missing required claims.");

  return {
    webToken,

    userName: tokenPayload.sub,

    role,

    expiresAt: tokenPayload.exp * 1000
  };
}

function storedAuthentication(): AuthenticationState | null
{
  const webToken = storedWebToken();

  if (webToken === null)

    return null;

  try
  {
    const authenticationState = createAuthenticationState(webToken);

    if (authenticationState.expiresAt <= Date.now())
    {
      removeStoredWebToken();

      return null;
    }

    return authenticationState;
  }
  catch
  {
    removeStoredWebToken();

    return null;
  }
}

export function AuthenticationContextProvider
(
  {
    children
  }:
  {
    children: ReactNode;
  }
)
{
  const [authentication, setAuthenticationState] =

    useState<AuthenticationState | null>(storedAuthentication);

  const clearAuthentication = useCallback(() =>
  {
    removeStoredWebToken();

    setAuthenticationState(null);
  }, []);

  useEffect(() =>
  {
    if (authentication === null)

      return;

    const expiresAt = authentication.expiresAt;

    let timeoutId: number | undefined;

    function expireAuthentication()
    {
      const expirationDelay = expiresAt - Date.now();

      if (expirationDelay <= 0)
      {
        clearAuthentication();

        return;
      }

      timeoutId = window.setTimeout(
        expireAuthentication,

        Math.min(
          expirationDelay,
          maximumTimeoutMilliseconds
        )
      );
    }

    expireAuthentication();

    return () =>
    {
      if (timeoutId !== undefined)

        window.clearTimeout(timeoutId);
    };
  }, [authentication, clearAuthentication]);

  function setAuthentication(webToken: string)
  {
    const authenticationState = createAuthenticationState(webToken);

    storeWebToken(webToken);

    setAuthenticationState(authenticationState);
  }

  return (
    <AuthenticationContext.Provider
      value={{
        webToken: authentication?.webToken ?? null,

        userName: authentication?.userName ?? null,

        role: authentication?.role ?? null,

        expiresAt: authentication?.expiresAt ?? null,

        isAuthenticated: authentication !== null,

        setAuthentication,

        clearAuthentication
      }}
    >
      {children}
    </AuthenticationContext.Provider>
  );
}