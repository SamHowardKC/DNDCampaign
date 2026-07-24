// The one place that knows how auth state is persisted in the browser.
// Everything else (login, logout, route guard, API client) goes through here,
// so there's a single source of truth instead of scattered localStorage calls.

const TOKEN_KEY = "jwt";
const USER_ID_KEY = "userID";
const USERNAME_KEY = "username";

export interface StoredAuth {
  token: string;
  userID: string;
  username: string;
}

export function saveAuth({ token, userID, username }: StoredAuth): void {
  localStorage.setItem(TOKEN_KEY, token);
  localStorage.setItem(USER_ID_KEY, userID);
  localStorage.setItem(USERNAME_KEY, username);
}

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY);
}

// Presence check only — this is a UX gate, not a security boundary. The server
// is the real authority: it rejects missing/expired/tampered tokens with a 401,
// which the API client turns into a forced logout.
export function isAuthenticated(): boolean {
  const token = getToken();
  return token !== null && token.trim() !== "";
}

export function logout(): void {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(USER_ID_KEY);
  localStorage.removeItem(USERNAME_KEY);
}
