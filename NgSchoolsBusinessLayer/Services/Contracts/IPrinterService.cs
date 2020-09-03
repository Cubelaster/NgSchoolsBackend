using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IPrinterService
    {
        Task<ActionResponse<PrinterDto>> GetById(int id);
        Task<ActionResponse<List<PrinterDto>>> GetAll();
        Task<ActionResponse<PrinterDto>> Update(PrinterDto entityDto);
        Task<ActionResponse<PrinterDto>> Insert(PrinterDto entityDto);
        Task<ActionResponse<PrinterDto>> Delete(int id);
    }
}
