namespace Library.Core.Constants
{
    public static class AppSettings
    {
        // File Settings
        public const string DataFileName = "data.json";
        
        // Validation Rules
        public const int MinTitleLength = 2;
        public const int MinYearValue = 0;
        
        // UI Settings
        public const int MaxTitleDisplayLength = 30;
        public const int MaxAuthorDisplayLength = 20;
        
        // Environment
        public static bool IsProduction => 
            Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Production";
    }
}
