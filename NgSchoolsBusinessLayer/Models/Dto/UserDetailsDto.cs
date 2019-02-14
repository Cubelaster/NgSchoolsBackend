using System;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class UserDetailsDto
    {
        public int? Id { get; set; }
        public Guid? UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? AvatarId { get; set; }
        public FileDto Avatar { get; set; }
        public int? SignatureId { get; set; }
        public FileDto Signature { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string Mobile2 { get; set; }
        public string Phone { get; set; }
        public string Oib { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public virtual CityDto City { get; set; }
        public int? CountryId { get; set; }
        public CountryDto Country { get; set; }
        public int? RegionId { get; set; }
        public RegionDto Region { get; set; }
        public string Profession { get; set; }
        public string Qualifications { get; set; }
        public string EmploymentPlace { get; set; }
        public string Bank { get; set; }
        public string Iban { get; set; }
        public bool PpEducation { get; set; }
        public string Authorization { get; set; }
        public string Certificates { get; set; }
        public string Notes { get; set; }
        public List<FileDto> TeacherFiles { get; set; }
    }
}
