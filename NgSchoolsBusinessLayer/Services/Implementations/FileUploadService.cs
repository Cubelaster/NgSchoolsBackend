using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;

        public FileUploadService(ILoggerService loggerService, IConfiguration configuration)
        {
            this.loggerService = loggerService;
            this.configuration = configuration;
        }

        public async Task<ActionResponse<string>> Upload(FileUploadRequest fileUploadRequest)
        {
            try
            {
                string uploadFolderName = configuration.GetValue<string>("UploadDestination").Replace("/", "");
                string ottResources = Path.Combine(Directory.GetCurrentDirectory(), uploadFolderName);
                string directoryPath = ottResources;
                string filePath = Path.Combine(directoryPath, fileUploadRequest.FileName);

                var bytes = Convert.FromBase64String(fileUploadRequest.FileString);

                using (var file = new FileStream(filePath, FileMode.Create))
                {
                    file.Write(bytes, 0, bytes.Length);
                    file.Flush();
                }

                return await ActionResponse<string>.ReturnSuccess(filePath.Replace(ottResources, "").Replace("\\", ""), "Datoteka uspješno učitana i spremljena.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog<FileUploadRequest>(ex, fileUploadRequest);
                return await ActionResponse<string>.ReturnError("Greška kod učitavanja datoteke.");
            }
        }
    }
}
