namespace Library.Core.Interfaces
{
    /// <summary>
    /// Provides file path resolution for data storage
    /// This abstraction allows different implementations for different environments
    /// </summary>
    public interface IPathProvider
    {
        /// <summary>
        /// Get the full path for a data file
        /// </summary>
        /// <param name="fileName">Name of the file (e.g., "data.json")</param>
        /// <returns>Full path to the file</returns>
        string GetDataPath(string fileName);
    }
}
