using System;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class InstitutionDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string InstitutionCode { get; set; }
        public string City { get; set; }
        public string InstitutionClassFirstPart { get; set; }
        public string InstitutionClassSecondPart { get; set; }
        public string InstitutionUrNumber { get; set; }
        public string Logo { get; set; }
        public Guid? PrincipalId { get; set; }
        public UserDto Principal { get; set; }
    }
}
