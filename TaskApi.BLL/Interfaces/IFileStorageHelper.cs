using Microsoft.AspNetCore.Http;

namespace TaskApi.BLL.Interfaces
{
    /// <summary>
    /// Utility for work with files
    /// </summary>
    public interface IFileStorageHelper
    { 
        string GenerateFileName(string fileName);

        string GetFileName(DateTime dateTime, Guid guid, string fileName);

        string CalculateHash(IFormFile file);

        string GetFileNameFromFilePath(string path);

        Guid GetIdFromFilePath(string path);
    }
}
