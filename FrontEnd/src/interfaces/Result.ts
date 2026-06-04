export interface ResultInterface<T> {
  success: boolean;
  error: string | null;
  data: T;
}

export interface FluentValidator {
  type: string;
  title: string;
  status: number;
  errors: {
    [key: string]: string[];
  };
  traceId: string;
}

export type ApiResponse<T> = ResultInterface<T> | FluentValidator;
