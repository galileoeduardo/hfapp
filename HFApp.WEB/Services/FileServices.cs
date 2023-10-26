using HFApp.WEB.Models.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Security;
using System.Text;
using System.Xml;
using System.Xml.Linq;
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

        public async Task<string?> GetJsonFromXML(Stream xmlBuffer)
        {
            XmlDocument doc = BufferedReadStream(xmlBuffer);

            XmlSerializer serializer = new XmlSerializer(typeof(CompNfseDto), new XmlRootAttribute("CompNfse"));
            byte[] xmlBytes = Encoding.UTF8.GetBytes(doc.OuterXml);
            using (StreamReader reader = new StreamReader(new MemoryStream(xmlBytes)))
            {
                CompNfseDto nfse = (CompNfseDto)serializer.Deserialize(reader);
                string json = nfse.ToJson(Newtonsoft.Json.Formatting.Indented);
                return await Task.FromResult<string?>(json.ToString());
            }
       }

        private XmlDocument? BufferedReadStream(Stream bufferXml)
        {
            try
            {
                using (StreamReader reader = new StreamReader(bufferXml))
                {
                    string strXml = reader.ReadToEnd();
                    strXml = strXml.Replace(" xmlns=\"http://www.abrasf.org.br/nfse.xsd\"", String.Empty);

                    XmlDocument xml = new XmlDocument();
                    xml.Load(new StringReader(strXml));
                    return xml;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}

