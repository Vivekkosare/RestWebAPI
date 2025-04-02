namespace RestWebAPI.Entities
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public string? Error { get; set; }

        protected Result(bool isSuccess, T value, string error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        protected Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public static Result<T> Success(T value) =>
            new Result<T>(true, value, null);
        public static Result<T> Success() =>
            new Result<T>(true);

        public static Result<T> Failure(string error) =>
            new Result<T>(false, default, error);
    }
}
