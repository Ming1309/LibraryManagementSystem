namespace Library.Core.Common
{
    /// <summary>
    /// Represents the result of an operation
    /// </summary>
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

        /// <summary>
        /// Create a successful result
        /// </summary>
        public static Result Success(string message)
            => new Result(true, message);

        /// <summary>
        /// Create a failed result
        /// </summary>
        public static Result Failure(string message, string errorCode)
            => new Result(false, message, errorCode);
    }

    /// <summary>
    /// Represents the result of an operation with data
    /// </summary>
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

        /// <summary>
        /// Create a successful result with data
        /// </summary>
        public static Result<T> Success(T data, string message)
            => new Result<T>(true, data, message);

        /// <summary>
        /// Create a failed result
        /// </summary>
        public static Result<T> Failure(string message, string errorCode)
            => new Result<T>(false, default, message, errorCode);
    }
}
