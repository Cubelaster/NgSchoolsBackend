using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Services.Contracts;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class FileUploadService : IFileUploadService
    {
        private readonly ILoggerService loggerService;

        public FileUploadService(ILoggerService loggerService)
        {
            this.loggerService = loggerService;
        }

        public async Task<ActionResponse<string>> Upload(FileUploadRequest fileUploadRequest)
        {
            try
            {
                Regex regex = null;

                if (fileUploadRequest.FileString.StartsWith("data:image", StringComparison.InvariantCulture))
                {
                    regex = new Regex("data:image\\/(?<extension>(png)|(jpg)|(jpeg)|(gif));base64,(?<payload>.+)");
                }
                else if (fileUploadRequest.FileString.StartsWith("data:application", StringComparison.InvariantCulture))
                {
                    regex = new Regex("data:application\\/(?<extension>(ttf));base64,(?<payload>.+)");
                }
                else
                {
                    return await ActionResponse<string>.ReturnError("File not in supported base 64 format");
                }

                string extension = regex.Match(fileUploadRequest.FileString).Groups["extension"].Value;
                string payload = regex.Match(fileUploadRequest.FileString).Groups["payload"].Value;

                string uploadFolderName = "uploads";
                string ottResources = Path.Combine(Directory.GetCurrentDirectory(), uploadFolderName);
                string fileName = $"{Guid.NewGuid().ToString().Replace("-", "") }.{extension}";
                string directoryPath = ottResources;
                string filePath = Path.Combine(directoryPath, fileName);

                var bytes = Convert.FromBase64String(payload);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var imageFile = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                return await ActionResponse<string>.ReturnSuccess(filePath.Replace(ottResources, "").Replace("\\", ""), "File uploaded successfully");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog<FileUploadRequest>(ex, fileUploadRequest);
                return await ActionResponse<string>.ReturnError("Some sort of fuckup. Try again.");
            }
        }
    }
}
