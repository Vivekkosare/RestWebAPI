using System.Net;

namespace RestWebAPI.Entities
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public string? Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        protected Result(bool isSuccess, T value, string error,
        HttpStatusCode statusCode)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            StatusCode = statusCode;
        }

        protected Result(bool isSuccess, HttpStatusCode statusCode)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
        }
        public static Result<T> Success(T value) =>
            new Result<T>(true, value, default!, HttpStatusCode.OK);
        public static Result<T> Success() =>
            new Result<T>(true, HttpStatusCode.OK);

        public static Result<T> Failure(string error, HttpStatusCode statusCode) =>
            new Result<T>(false, default!, error, statusCode);
    }
}
