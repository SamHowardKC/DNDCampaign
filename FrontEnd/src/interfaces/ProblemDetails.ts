// The single error shape the backend returns for every failed request.
// `errors` is only present for validation failures (FluentValidation).
export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  errors?: {
    [key: string]: string[];
  };
  traceId?: string;
}
