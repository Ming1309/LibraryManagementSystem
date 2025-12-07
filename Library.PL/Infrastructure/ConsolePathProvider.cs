using Library.Core.Interfaces;
using System.Reflection;

namespace Library.PL.Infrastructure
{
    /// <summary>
    /// Runtime path provider for console application
    /// Resolves data file paths based on execution environment
    /// </summary>
    public class ConsolePathProvider : IPathProvider
    {
        private readonly string _dataDirectory;

        public ConsolePathProvider()
        {
#if DEBUG
            // Development: Store data in solution's Data folder or DAL folder
            _dataDirectory = GetDevelopmentDataDirectory();
#else
            // Production: Store data in application directory
            _dataDirectory = AppDomain.CurrentDomain.BaseDirectory;
#endif
        }

        public string GetDataPath(string fileName)
        {
            string fullPath = Path.Combine(_dataDirectory, fileName);
            
            // Ensure directory exists
            string? directory = Path.GetDirectoryName(fullPath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return fullPath;
        }

        private string GetDevelopmentDataDirectory()
        {
            // Get the executing assembly location
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string? assemblyDir = Path.GetDirectoryName(assemblyLocation);

            if (assemblyDir != null)
            {
                // Navigate up from Library.PL/bin/Debug/net10.0 to solution root
                string solutionRoot = Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", ".."));
                
                // Store in Library.DAL folder for development
                string dalFolder = Path.Combine(solutionRoot, "Library.DAL");
                
                return dalFolder;
            }

            // Fallback to application directory
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
