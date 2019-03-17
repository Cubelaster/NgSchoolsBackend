using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Configuration;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        #region Ctors and Members

        private readonly IConverter converter;
        private readonly IConfiguration configuration;

        public PdfGeneratorService(IConverter converter, IConfiguration configuration)
        {
            this.converter = converter;
            this.configuration = configuration;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<FileDto>> GeneratePdf()
        {
            string uploadFolderName = configuration.GetValue<string>("UploadDestination").Replace("/", "");
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), uploadFolderName);
            string filePath = Path.Combine(directoryPath, "Pdf_Test.pdf");

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
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
                        <html>
                            <head>
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
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };


            byte[] file = converter.Convert(pdf);

            return await ActionResponse<FileDto>.ReturnSuccess();
        }
    }
}
