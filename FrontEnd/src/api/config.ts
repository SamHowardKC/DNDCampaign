// Single source of truth for the backend base URL.
// Override per-environment with a Vite env var (VITE_API_BASE) if you ever need
// to point at a local backend; otherwise it defaults to the deployed API.
const rawBase =
  (import.meta.env.VITE_API_BASE as string | undefined) ??
  "https://dndcampaign.onrender.com";

// Trim a trailing slash so callers can safely write `${API_BASE}/api/...`.
export const API_BASE = rawBase.replace(/\/$/, "");
