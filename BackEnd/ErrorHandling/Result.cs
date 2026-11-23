using System.Diagnostics.CodeAnalysis;

namespace BackEnd.ErrorHandling
{
    // Why an operation failed. The service decides this (it knows why), and
    // ResultExtensions.ToProblem maps it to an HTTP status code in one place.
    public enum ErrorType
    {
        Validation,
        Unauthorized,
        Forbidden,
        NotFound,
        Conflict
    }

    // Internal service -> controller result. Never serialized to the client:
    // controllers return Data on success, or a ProblemDetails via ToProblem on failure.
    public class Result<T>
    {
        // MemberNotNullWhen lets the compiler know Data is set once Success is checked,
        // and Error/ErrorType are set once !Success is checked.
        [MemberNotNullWhen(true, nameof(Data))]
        [MemberNotNullWhen(false, nameof(Error), nameof(ErrorType))]
        public bool Success { get; }
        public T? Data { get; }
        public string? Error { get; }
        public ErrorType? ErrorType { get; }

        // Private so the only way to build a Result is through Ok or Fail,
        // which rules out states like "succeeded but has an error".
        private Result(bool success, T? data, string? error, ErrorType? errorType)
        {
            Success = success;
            Data = data;
            Error = error;
            ErrorType = errorType;
        }

        public static Result<T> Ok(T data) =>
            new Result<T>(true, data, null, null);

        public static Result<T> Fail(ErrorType errorType, string error) =>
            new Result<T>(false, default, error, errorType);
    }
}
