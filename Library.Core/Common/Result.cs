namespace Library.Core.Common
{
    // Represents the result of an operation
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public string? ErrorCode { get; }

        private Result(bool isSuccess, string message, string? errorCode = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            ErrorCode = errorCode;
        }

        // Create a successful result
        public static Result Success(string message)
            => new Result(true, message);

        // Create a failed result
        public static Result Failure(string message, string errorCode)
            => new Result(false, message, errorCode);
    }

    // Represents the result of an operation with data
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string Message { get; }
        public string? ErrorCode { get; }

        private Result(bool isSuccess, T? data, string message, string? errorCode = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            ErrorCode = errorCode;
        }

        // Create a successful result with data

        public static Result<T> Success(T data, string message)
            => new Result<T>(true, data, message);

        // Create a failed result
        public static Result<T> Failure(string message, string errorCode)
            => new Result<T>(false, default, message, errorCode);
    }
}
