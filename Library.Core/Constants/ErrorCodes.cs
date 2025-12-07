namespace Library.Core.Constants
{
    /// <summary>
    /// Error codes for different error types
    /// </summary>
    public static class ErrorCodes
    {
        // Validation Errors
        public const string ValidationError = "VALIDATION_ERROR";
        public const string TitleEmpty = "TITLE_EMPTY";
        public const string TitleTooShort = "TITLE_TOO_SHORT";
        public const string YearInvalid = "YEAR_INVALID";
        public const string YearFuture = "YEAR_FUTURE";

        // Not Found Errors
        public const string NotFound = "NOT_FOUND";
        public const string BookNotExist = "BOOK_NOT_EXIST";

        // System Errors
        public const string SystemError = "SYSTEM_ERROR";
        public const string SaveError = "SAVE_ERROR";
        public const string DeleteError = "DELETE_ERROR";
    }
}
