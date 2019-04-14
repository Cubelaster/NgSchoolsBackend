using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class Student : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int? PhotoId { get; set; }
        public UploadedFile Photo { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Mentor { get; set; }
        public string Oib { get; set; }
        public int? CityOfBirthId { get; set; }
        public virtual City CityOfBirth { get; set; }
        public int? CountryOfBirthId { get; set; }
        public virtual Country CountryOfBirth { get; set; }
        public string Mob { get; set; }
        public int? RegionOfBirthId { get; set; }
        public virtual Region RegionOfBirth { get; set; }
        public int? MunicipalityOfBirthId { get; set; }
        public virtual Municipality MunicipalityOfBirth { get; set; }
        public string Citizenship { get; set; }
        public string Proffesion { get; set; }
        public bool IdCard { get; set; }
        public string FathersFullName { get; set; }
        public string MothersFullName { get; set; }
        public string Gender { get; set; }
        public string AddressStreet { get; set; }

        public int? AddressCountryId { get; set; }
        public virtual Country AddressCountry { get; set; }
        public int? AddressCityId { get; set; }
        public virtual City AddressCity { get; set; }
        public int? AddressRegionId { get; set; }
        public virtual Region AddressRegion { get; set; }
        public int? AddressMunicipalityId { get; set; }
        public virtual Municipality AddressMunicipality { get; set; }

        public string AddressMuncipality { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool Employed { get; set; }
        public string EmployerName { get; set; }
        public bool EmployerApplicationAttendant { get; set; }       
        public string EmployerOib { get; set; }
        public bool EducationContract { get; set; }
        public bool FinalExam { get; set; }
        public bool CompletedSchoolCertificate { get; set; }
        public bool PractiacalTrainingContract { get; set; }
        public bool DoctorCertification { get; set; }
        public bool DriversLicence { get; set; }
        public bool KnowledgeTest { get; set; }
        public bool CitizenshipCertificate { get; set; }
        public bool BirthCertificate { get; set; }
        public bool PracticeDiary { get; set; }
        public DateTime? EnrolmentDate { get; set; }
        public string FinishedSchool { get; set; }
        public string CertificateNumber { get; set; }
        public string Vocation { get; set; }
        public string SchoolLevel { get; set; }
        public bool Gdpr { get; set; }
        public string Notes { get; set; }

        public int? EmployerId { get; set; }
        public virtual BusinessPartner Employer { get; set; }

        public virtual ICollection<StudentsInGroups> StudentsInGroups { get; set; }
        public virtual ICollection<StudentFiles> Files { get; set; }
    }
}
