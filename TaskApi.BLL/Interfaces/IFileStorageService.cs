using Microsoft.AspNetCore.Http;
using TaskApi.BLL.Dto;

namespace TaskApi.BLL.Interfaces
{
    /// <summary>
    /// Service for work with external file storage
    /// </summary>
    public interface IFileStorageService
    {
        Task<IEnumerable<TicketFileDto>> AddFiles(ICollection<IFormFile> files, Dictionary<string, string> fileHashes);

        Task<IEnumerable<TicketFileDto>> AddNewFiles(ICollection<IFormFile> files);

        Task<IEnumerable<TicketFileDto>> UpdateFiles(ICollection<IFormFile> files, IList<string> fileNames);

        Task RemoveFiles(IList<string> fileNames);
    }
}
