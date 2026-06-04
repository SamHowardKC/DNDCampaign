namespace BackEnd.ErrorHandling
{
    public class Result<T>
    {
        public bool Success { get; init; } = false;
        public string Error { get; init; } = "Default Error";
        public T Data { get; init; } = default!;

        public static Result<T> Ok(T data) =>
            new Result<T> { Success = true, Data = data };

        public static Result<T> Fail(string error) =>
            new Result<T> { Success = false, Error = error };
    }

}
