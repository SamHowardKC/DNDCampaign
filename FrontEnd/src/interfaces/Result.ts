export interface ResultInterface<T> {
  success: boolean;
  Error: string | null;
  Data: T;
}