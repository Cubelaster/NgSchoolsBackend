using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Services.Contracts;
using System;
using System.IO;
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
                string uploadFolderName = "uploads";
                string ottResources = Path.Combine(Directory.GetCurrentDirectory(), uploadFolderName);
                string directoryPath = ottResources;
                string filePath = Path.Combine(directoryPath, fileUploadRequest.FileName);

                var bytes = Convert.FromBase64String(fileUploadRequest.FileString);

                using (var file = new FileStream(filePath, FileMode.Create))
                {
                    file.Write(bytes, 0, bytes.Length);
                    file.Flush();
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
