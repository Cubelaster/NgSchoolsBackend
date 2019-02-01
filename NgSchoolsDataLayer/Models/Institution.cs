using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class Institution : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string InstitutionCode { get; set; }
        [Required]
        public int CityId { get; set; }
        public virtual City City { get; set; }
        [Required]
        public string InstitutionClassFirstPart { get; set; }
        [Required]
        public string InstitutionClassSecondPart { get; set; }
        [Required]
        public string InstitutionUrNumber { get; set; }
        public string Address { get; set; }

        public int? LogoId { get; set; }
        public virtual UploadedFile Logo { get; set; }
        public Guid? PrincipalId { get; set; }
        public User Principal { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public int RegionId { get; set; }
        public virtual Region Region { get; set; }
    }
}
