namespace HFApp.WEB.Services
{
    public interface IFileServices
    {
        Task<bool> UploadFileAsync(Stream file, string fileName, string extension);
    }
}