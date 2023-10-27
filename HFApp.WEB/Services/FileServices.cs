using HFApp.WEB.Models.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace HFApp.WEB.Services
{
    public class FileServices : IFileServices
    {

        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadPath = "";
        
        private XmlNode root;


        public FileServices(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadPath = Path.Combine(_environment.ContentRootPath, "Uploads");
        }
        public async Task<bool> UploadFileAsync(Stream file, string fileNameOutput)
        {
            try
            {
                if (!System.IO.Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }
                string outPath = Path.Combine(_uploadPath, fileNameOutput);
                using (FileStream outfile = new FileStream(outPath, FileMode.Create))
                {
                    await file.CopyToAsync(outfile);
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

        public async Task<string> DeserializeObject(string fileName)
        {
            string filePath = @$"{_uploadPath}\{fileName}";

            FileStream fs = new FileStream(filePath, FileMode.Open);

            XmlReaderSettings nfseSettings = new XmlReaderSettings();
            nfseSettings.ValidationType = ValidationType.Schema;
            nfseSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            nfseSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            nfseSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            nfseSettings.ConformanceLevel = ConformanceLevel.Fragment;
            nfseSettings.Async = true;
            nfseSettings.ToJson(Newtonsoft.Json.Formatting.Indented);
            nfseSettings.ValidationEventHandler += new ValidationEventHandler(booksSettingsValidationEventHandler);

            using (XmlReader reader = XmlReader.Create(fs, nfseSettings))
            {
                StringBuilder sb = new StringBuilder("{");
                int open = 1;
                int close = 0;
                int next = 0;
                
                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            next = 0;
                            if(sb.ToString().EndsWith(':')) {
                                sb.Append('{');
                                open++;
                            }
                            sb.Append($"\"{reader.Name}\":");
                            next++;
                            break;
                        case XmlNodeType.Text:
                            string value = await reader.GetValueAsync();
                            sb.Append($"\"{value}\",");
                            next++;
                            break;
                        case XmlNodeType.EndElement:
                            next++;
                            if (next % 4 == 0)
                            {
                                sb.Remove(sb.Length - 1, 1);
                                sb.Append("},");
                                close++;
                            }
                            break;
                        default:
                            Console.WriteLine("Other node {0} with value {1}",
                                            reader.NodeType, reader.Value);
                            break;
                    }
                }

                if (sb.ToString().EndsWith(','))
                {
                    sb.Remove(sb.Length - 1, 1);
                }

                int length = open - close;
                for (int i = 0; i < length; i++)
                {
                    sb.Append('}');
                }
                sb.Replace("\t",string.Empty);
                sb.Replace("\n",string.Empty);
                sb.Replace("\r",string.Empty);
                return sb.ToString();
            }
        }

        public static void booksSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Console.Write("WARNING: ");
                Console.WriteLine(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.Write("ERROR: ");
                Console.WriteLine(e.Message);
            }
        }

        
    }
}

