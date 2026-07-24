import type { ReactNode } from "react";
import { Navigate } from "react-router-dom";
import { isAuthenticated } from "../../auth/auth";

// Wraps routes that require a logged-in user. If there's no token, we redirect
// to /login instead of rendering the page. This is a client-side convenience
// (no more empty dashboard on a public URL) — the backend still enforces auth
// on every request, so this is not what actually protects the data.
export default function ProtectedRoute({ children }: { children: ReactNode }) {
  if (!isAuthenticated()) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
}
