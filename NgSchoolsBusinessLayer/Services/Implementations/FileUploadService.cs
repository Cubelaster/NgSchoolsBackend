using AutoMapper;
using Microsoft.Extensions.Configuration;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public FileUploadService(IConfiguration configuration,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ActionResponse<FileDto>> Upload(FileUploadRequest fileUploadRequest)
        {
            try
            {
                string uploadFolderName = configuration.GetValue<string>("UploadDestination").Replace("/", "");
                string ottResources = Path.Combine(Directory.GetCurrentDirectory(), uploadFolderName);
                string directoryPath = ottResources;
                string filePath = Path.Combine(directoryPath, fileUploadRequest.FileName);

                bool fileExists = true;
                int iteration = 0;
                string filePathNew = filePath;

                while (fileExists)
                {
                    if (File.Exists(filePathNew))
                    {
                        iteration++;
                        var extension = filePath.Substring(filePath.LastIndexOf("."));
                        filePathNew = filePath.Substring(0, filePath.LastIndexOf(".")) + "_" + Guid.NewGuid().ToString() + extension;
                        continue;
                    }
                    filePath = filePathNew;
                    fileExists = false;
                }

                var bytes = Convert.FromBase64String(fileUploadRequest.FileString);

                using (var file = new FileStream(filePath, FileMode.Create))
                {
                    file.Write(bytes, 0, bytes.Length);
                    file.Flush();
                }

                UploadedFile uploadedFile = new UploadedFile
                {
                    FileName = filePath.Replace(ottResources, "").Replace("\\", "")
                };

                unitOfWork.GetGenericRepository<UploadedFile>().Add(uploadedFile);
                unitOfWork.Save();

                return await ActionResponse<FileDto>.ReturnSuccess(mapper.Map<UploadedFile, FileDto>(uploadedFile), "Datoteka uspješno učitana i spremljena.");
            }
            catch (Exception)
            {
                return await ActionResponse<FileDto>.ReturnError("Greška kod učitavanja datoteke.");
            }
        }
    }
}
