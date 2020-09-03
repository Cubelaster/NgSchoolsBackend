using AutoMapper;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class PrinterService : IPrinterService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public PrinterService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<PrinterDto>> GetById(int id)
        {
            try
            {
                var query = unitOfWork.GetGenericRepository<Printer>()
                    .ReadAllActiveAsQueryable()
                    .Where(e => e.Id == id);

                var entity = mapper.ProjectTo<PrinterDto>(query).FirstOrDefault();

                return await ActionResponse<PrinterDto>.ReturnSuccess(entity);
            }
            catch (Exception)
            {
                return await ActionResponse<PrinterDto>.ReturnError("Greška prilikom dohvata printera.");
            }
        }

        public async Task<ActionResponse<List<PrinterDto>>> GetAll()
        {
            try
            {
                var query = unitOfWork.GetGenericRepository<Printer>()
                        .ReadAllActiveAsQueryable();

                var entities = mapper.ProjectTo<PrinterDto>(query).ToList();

                return await ActionResponse<List<PrinterDto>>.ReturnSuccess(entities);
            }
            catch (Exception)
            {
                return await ActionResponse<List<PrinterDto>>.ReturnError("Greška prilikom dohvata svih printera.");
            }
        }

        public async Task<ActionResponse<PrinterDto>> Insert(PrinterDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<PrinterDto, Printer>(entityDto);
                unitOfWork.GetGenericRepository<Printer>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<PrinterDto>
                    .ReturnSuccess(mapper.Map<Printer, PrinterDto>(entityToAdd));
            }
            catch (Exception)
            {
                return await ActionResponse<PrinterDto>.ReturnError("Greška prilikom kreiranja printera.");
            }
        }

        public async Task<ActionResponse<PrinterDto>> Update(PrinterDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<PrinterDto, Printer>(entityDto);
                unitOfWork.GetGenericRepository<Printer>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<PrinterDto>
                    .ReturnSuccess(mapper.Map<Printer, PrinterDto>(entityToUpdate));
            }
            catch (Exception)
            {
                return await ActionResponse<PrinterDto>.ReturnError("Greška prilikom ažuriranja printera.");
            }
        }

        public async Task<ActionResponse<PrinterDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Printer>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<PrinterDto>.ReturnSuccess(null, "Brisanje printera uspješno.");
            }
            catch (Exception)
            {
                return await ActionResponse<PrinterDto>.ReturnError("Greška prilikom brisanja printera.");
            }
        }
    }
}
