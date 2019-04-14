using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Configuration;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System.IO;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        #region Ctors and Members

        private readonly IConverter converter;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PdfGeneratorService(IConverter converter, IConfiguration configuration,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.converter = converter;
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<FileDto>> GeneratePdf(string fileName)
        {
            string uploadFolderName = configuration.GetValue<string>("UploadDestination").Replace("/", "");
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), uploadFolderName);
            string filePath = Path.Combine(directoryPath, fileName);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                DocumentTitle = fileName,
                Out = filePath
            };

            //var bytes = Convert.FromBase64String(fileUploadRequest.FileString);

            //using (var file = new FileStream(filePath, FileMode.Create))
            //{
            //    file.Write(bytes, 0, bytes.Length);
            //    file.Flush();
            //}

            //var objectSettings = new ObjectSettings
            //{
            //    Page = "https://www.google.com/"
            //};

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = @"
                    <html lang='en'>
                    <head>
                        <style>
                            .header {
                                text-align: center;
                                color: green;
                                padding-bottom: 35px;
                            }
 
                            table {
                                width: 80%;
                                border-collapse: collapse;
                            }
 
                            td, th {
                                border: 1px solid gray;
                                padding: 15px;
                                font-size: 22px;
                                text-align: center;
                            }
 
                            table th {
                                background-color: green;
                                color: white;
                            }
                        </style>
                        </head>
                        <body>
                            <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Name</th>
                                        <th>LastName</th>
                                        <th>Age</th>
                                        <th>Gender</th>
                                    </tr>
                                </table>
                        </body>
                        </html>",
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontName = "Arial", FontSize = 0, Right = "", Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 0, Center = "", Line = false }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            byte[] file = converter.Convert(pdf);

            UploadedFile generatedFile = new UploadedFile
            {
                FileName = pdf.GlobalSettings.DocumentTitle
            };

            unitOfWork.GetGenericRepository<UploadedFile>().Add(generatedFile);
            unitOfWork.Save();

            return await ActionResponse<FileDto>.ReturnSuccess(mapper.Map<UploadedFile, FileDto>(generatedFile), "Datoteka uspješno generirana i spremljena.");
        }
    }
}
