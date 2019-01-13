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
        public string City { get; set; }
        [Required]
        public string InstitutionClassFirstPart { get; set; }
        [Required]
        public string InstitutionClassSecondPart { get; set; }
        [Required]
        public string InstitutionUrNumber { get; set; }

        public int? LogoId { get; set; }
        public UploadedFile Logo { get; set; }
        public Guid? PrincipalId { get; set; }
        public User Principal { get; set; }
    }
}
