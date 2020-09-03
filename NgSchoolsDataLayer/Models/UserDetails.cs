using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class UserDetails : DatabaseEntity
    {
        public UserDetails() : base() {}

        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int? AvatarId { get; set; }
        public UploadedFile Avatar { get; set; }
        public int? SignatureId { get; set; }
        public UploadedFile Signature { get; set; }
        public string Title { get; set; }
        public string TitlePrefix { get; set; }
        public string Mobile { get; set; }
        public string Mobile2 { get; set; }
        public string Phone { get; set; }
        public string Oib { get; set; }
        public string Address { get; set; }

        public int? CityId { get; set; }
        public virtual City City { get; set; }

        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        public int? RegionId { get; set; }
        public virtual Region Region { get; set; }

        public int? MunicipalityId { get; set; }
        public virtual Municipality Municipality { get; set; }

        public string Profession { get; set; }
        public string Qualifications { get; set; }
        public string EmploymentPlace { get; set; }
        public string Bank { get; set; }
        public string Iban { get; set; }
        public bool PpEducation { get; set; }
        public string Authorization { get; set; }
        public string Certificates { get; set; }
        public string Notes { get; set; }

        public Guid? UserId { get; set; }
        public User User { get; set; }

        public ICollection<TeacherFile> TeacherFiles { get; set; }
    }
}
