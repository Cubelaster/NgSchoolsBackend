using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PrinterController : ControllerBase
    {
        #region Ctors and Members

        private readonly IPrinterService printerService;

        public PrinterController(IPrinterService printerService)
        {
            this.printerService = printerService;
        }

        #endregion Ctors and Members

        [HttpPost]
        public async Task<ActionResponse<PrinterDto>> GetById(SimpleRequestBase request)
        {
            return await printerService.GetById(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<PrinterDto>> Insert([FromBody] PrinterDto entityDto)
        {
            return await printerService.Insert(entityDto);
        }

        [HttpPost]
        public async Task<ActionResponse<PrinterDto>> Update([FromBody] PrinterDto entityDto)
        {
            return await printerService.Update(entityDto);
        }

        [HttpPost]
        public async Task<ActionResponse<List<PrinterDto>>> GetAll()
        {
            return await printerService.GetAll();
        }

        [HttpPost]
        public async Task<ActionResponse<PrinterDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await printerService.Delete(request.Id);
        }
    }
}
