using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IInstitutionService
    {
        Task<ActionResponse<InstitutionDto>> GetInstitution();
        Task<ActionResponse<InstitutionDto>> Update(InstitutionDto institution);
        Task<ActionResponse<InstitutionDto>> Insert(InstitutionDto institution);
        Task<ActionResponse<InstitutionDto>> Delete(int id);
    }
}