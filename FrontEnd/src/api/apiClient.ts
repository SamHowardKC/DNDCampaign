import { API_BASE } from "./config";
import { getToken, logout } from "../auth/auth";

// Thrown when the server rejects our token (401). The stale token is cleared
// before this is thrown, so callers just need to send the user back to /login.
export class UnauthorizedError extends Error {
  constructor(message = "Session expired. Please log in again.") {
    super(message);
    this.name = "UnauthorizedError";
  }
}

// Authenticated fetch for protected endpoints. Attaches the Bearer token and
// centralizes 401 handling so no component has to remember to do either.
export async function authFetch(
  path: string,
  options: RequestInit = {}
): Promise<Response> {
  const token = getToken();
  const headers = new Headers(options.headers);
  if (token) headers.set("Authorization", `Bearer ${token}`);

  const res = await fetch(`${API_BASE}${path}`, { ...options, headers });

  if (res.status === 401) {
    // Token missing, expired, or invalid — drop it and let the caller redirect.
    logout();
    throw new UnauthorizedError();
  }

  return res;
}

// Pulls a human-readable message out of an error response, coping with both
// shapes the backend can return:
//   - Result<T> failures:            { success, error, data }
//   - FluentValidation ProblemDetails: { errors: { field: [msg, ...] }, title }
export async function extractError(res: Response): Promise<string> {
  try {
    const body = await res.json();

    if (typeof body?.error === "string" && body.error.trim() !== "") {
      return body.error;
    }

    if (body?.errors && typeof body.errors === "object") {
      const first = Object.values(body.errors)[0];
      if (Array.isArray(first) && first.length > 0) return String(first[0]);
    }

    if (typeof body?.title === "string") return body.title;
  } catch {
    // Body wasn't JSON — fall through to a generic message.
  }

  return `Request failed (${res.status})`;
}
