namespace Library.Core.Constants
{
    public static class Messages
    {
        // Success Messages
        public const string Success = "Success";
        public const string SuccessAddBook = "Success: Book has been added to the system.";
        public const string SuccessUpdateBook = "Success: Book information has been updated.";
        public const string SuccessDeleteBook = "Success: Book has been removed from the system.";

        // Error Messages - Validation
        public const string ErrorTitleEmpty = "Error: Book title cannot be empty!";
        public const string ErrorTitleTooShort = "Error: Book title must be at least 2 characters!";
        public const string ErrorTitleTooShortUpdate = "Error: Book title is too short!";
        public const string ErrorYearInvalid = "Error: Publish year must be greater than 0!";
        public const string ErrorYearFuture = "Error: Publish year cannot be greater than current year ({0})!";
        public const string ErrorYearInvalidUpdate = "Error: Invalid publish year!";

        // Error Messages - Not Found
        public const string ErrorBookNotExist = "Error: Book does not exist in the system.";
        public const string ErrorBookNotFound = "Error: Book not found!";

        // Error Messages - System
        public const string ErrorSaveFile = "System error: Cannot save to file.";
        public const string ErrorDeleteFile = "Error: Cannot delete file.";
        public const string ErrorReadFile = "Error reading data file: {0}";
        public const string ErrorWriteFile = "Error writing data file: {0}";

        // Error Messages - Input (PL Layer)
        public const string ErrorIdMustBeNumber = "Error: ID must be a number!";
        public const string ErrorYearMustBeNumber = "Error: Year must be a number!";
        public const string ErrorInvalidYearInput = "Invalid year input, will keep old year.";

        // UI Messages
        public const string DeleteCancelled = "Delete operation cancelled.";
        public const string InvalidFunction = "Invalid function. Please select again!";
        public const string ExitProgram = "Exiting program...";
        public const string SearchNoResults = "No books found matching your search.";
        public const string ListEmpty = "List is empty!";
    }
}
