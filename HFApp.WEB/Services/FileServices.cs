using System.IO;

namespace HFApp.WEB.Services
{
    public class FileServices : IFileServices
    {
        public async Task<bool> UploadFileAsync(Stream file, string fileName, string extension)
        {
            string strFolder = System.IO.Path.GetTempPath();
            if (!System.IO.Directory.Exists(strFolder))
            {
                Directory.CreateDirectory(strFolder);
            }
            string outPath = strFolder + "" + fileName + "." + extension;
            using (StreamWriter outfile = new StreamWriter(outPath))
            {
                outfile.Write(file);
            }
            
            return await Task.FromResult(true);
        }
    }
}
