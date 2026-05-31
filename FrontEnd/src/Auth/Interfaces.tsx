export interface AuthResponse {
  Token: string;
  UserID: string;
  Username: string;
}

export interface Result<T> {
  success: boolean;
  Error: string | null;
  Data: T;
}