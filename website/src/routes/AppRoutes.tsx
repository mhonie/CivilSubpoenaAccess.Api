import {
  Outlet,
  Route,
  Routes
} from "react-router-dom";

import {
  ChartContextProvider
} from "../features/charts/ChartContextProvider";

import {
  fixEvidenceRoles,
  roleAdministrationRoles
} from "../features/authentication/rolePermissions";

import {
  ReportsContextProvider
} from "../features/reports/ReportsContextProvider";

import {
  ReviewQueueContextProvider
} from "../features/review-queue/ReviewQueueContextProvider";

import SubpoenaOrderDetails from

  "../features/review-queue/SubpoenaOrderDetails";

import Transactions from "../features/transactions/Transactions";

import Administration from "../pages/Admin";
import Charts from "../pages/Charts";
import FixEvidence from "../pages/Evidence";
import Home from "../pages/Home";
import Login from "../pages/Login";
import Queue from "../pages/Queue";
import Reports from "../pages/Reports";

import ProtectedRoute from "./ProtectedRoute";


export default function AppRoutes()
{
  return (
    <Routes>
      <Route
        path="/"
        element={<Login />}
      />

      <Route element={<ProtectedRoute />}>
        <Route
          path="/home"
          element={<Home />}
        />

        <Route
          path="/reports"
          element={
            <ReportsContextProvider>
              <Reports />
            </ReportsContextProvider>
          }
        />

        <Route
          path="/reports/transactions/:reportType/:category"
          element={
            <ReportsContextProvider>
              <Transactions />
            </ReportsContextProvider>
          }
        />

        <Route
          path="/charts"
          element={
            <ChartContextProvider>
              <Charts />
            </ChartContextProvider>
          }
        />

        <Route
          element={
            <ReviewQueueContextProvider>
              <Outlet />
            </ReviewQueueContextProvider>
          }
        >
          <Route
            path="/queue"
            element={<Queue />}
          />

          <Route
            path="/queue/:transactionId"
            element={<SubpoenaOrderDetails />}
          />
        </Route>

        <Route
          element={
            <ProtectedRoute allowedRoles={fixEvidenceRoles} />
          }
        >
          <Route
            path="/fix-evidence"
            element={<FixEvidence />}
          />
        </Route>

        <Route
          element={
            <ProtectedRoute
              allowedRoles={roleAdministrationRoles}
            />
          }
        >
          <Route
            path="/admin"
            element={<Administration />}
          />
        </Route>
      </Route>
    </Routes>
  );
}