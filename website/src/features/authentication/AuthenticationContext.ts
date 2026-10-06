import { createContext } from "react";


export type UserRole =

  "Certification"

  | "Finance"

  | "Admin"

  | "SuperAdmin";

export type AuthenticationContextType = {
  webToken: string | null;
  userName: string | null;
  role: UserRole | null;
  expiresAt: number | null;
  isAuthenticated: boolean;
  setAuthentication: (webToken: string) => void;
  clearAuthentication: () => void;
};

export const AuthenticationContext =

  createContext<AuthenticationContextType | null>(null);