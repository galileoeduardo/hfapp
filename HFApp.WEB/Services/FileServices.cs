using HFApp.WEB.Models.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace HFApp.WEB.Services
{
    public class FileServices : IFileServices
    {

        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadPath = "";


        public FileServices(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadPath = Path.Combine(_environment.ContentRootPath, "Uploads");
        }
        public async Task<bool> UploadFileAsync(Stream file, string fileName, string extension)
        {
            try
            {
                if (!System.IO.Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }
                string outPath = Path.Combine(_uploadPath, $"{fileName}.{extension}");
                using (StreamWriter outfile = new StreamWriter(outPath))
                {
                    outfile.Write(file);
                }

                return await Task.FromResult(true);

            }
            catch (Exception ex)
            {

                return await Task.FromResult(true);
            }

        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            string filePath = @$"{_uploadPath}\{fileName}";

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return await Task.FromResult(true);

                }
                else
                {
                    return await Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<FileStreamResult?> Download(string fileName, string origFilename)
        {

            string filePath = @$"{_uploadPath}\{fileName}";

            try
            {
                if (File.Exists(filePath))
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    var result = new FileStreamResult(fileStream, "application/octet-stream");
                    result.FileDownloadName = origFilename;

                    return await Task.FromResult<FileStreamResult?>(result);
                }

            }
            catch (Exception ex)
            {

                return await Task.FromResult<FileStreamResult?>(null);
            }

            return await Task.FromResult<FileStreamResult?>(null);
        }

        public async Task<CompNfseDto?> GetJsonFromXML(string xml)
        {
            try
            {
                var jsonObject = JsonConvert.DeserializeObject<CompNfseDto>(xml);
                return await Task.FromResult<CompNfseDto?>(jsonObject);
            }
            catch (Exception ex)
            {

                return await Task.FromResult<CompNfseDto?>(null);
            }
        }
    }
}

