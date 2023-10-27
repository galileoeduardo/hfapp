using Microsoft.AspNetCore.Mvc;

namespace HFApp.WEB.Services
{
    public interface IFileServices
    {
        Task<bool> UploadFileAsync(Stream file, string fileNameOutput);
        Task<bool> DeleteFileAsync(string fileName);
        Task<FileStreamResult> Download(string fileName, string origFilename);
        Task<string> DeserializeObject(string fileName);
    }
}