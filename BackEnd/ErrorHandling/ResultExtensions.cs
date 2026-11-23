using Microsoft.AspNetCore.Mvc;

namespace BackEnd.ErrorHandling
{
    public static class ResultExtensions
    {
        // Turns a failed Result into a ProblemDetails response, the same error shape
        // the framework uses for validation errors, 401s, 429s and unhandled exceptions.
        public static IActionResult ToProblem<T>(this ControllerBase controller, Result<T> result)
        {
            if (result.Success)
                throw new InvalidOperationException("ToProblem was called on a successful Result.");

            var statusCode = result.ErrorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            return controller.Problem(detail: result.Error, statusCode: statusCode);
        }
    }
}
