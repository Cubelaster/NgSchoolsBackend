using System;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class InstitutionDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string InstitutionCode { get; set; }
        public int? CityId { get; set; }
        public CityDto City { get; set; }
        public string InstitutionClassFirstPart { get; set; }
        public string InstitutionClassSecondPart { get; set; }
        public string InstitutionUrNumber { get; set; }
        public string Address { get; set; }
        public FileDto Logo { get; set; }
        public Guid? PrincipalId { get; set; }
        public UserDto Principal { get; set; }
        public int? CountryId { get; set; }
        public CountryDto Country { get; set; }
        public int? RegionId { get; set; }
        public RegionDto Region { get; set; }
        public List<FileDto> Files { get; set; }
    }
}
