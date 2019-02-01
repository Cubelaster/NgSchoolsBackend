﻿using NgSchoolsBusinessLayer.Enums;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.Student)]
    public class StudentDto
    {
        public int Id { get; set; }
        public int? PhotoId { get; set; }
        public FileDto Photo { get; set; }
        [Searchable]
        public string FirstName { get; set; }
        [Searchable]
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string Mentor { get; set; }
        public string Oib { get; set; }
        public string Pob { get; set; }
        public string Cob { get; set; }
        public string Mob { get; set; }
        public string Couob { get; set; }
        public string Citizenship { get; set; }
        public string Proffesion { get; set; }
        public bool IdCard { get; set; }
        public string FathersFullName { get; set; }
        public string MothersFullName { get; set; }
        public string Gender { get; set; }
        public string AddressStreet { get; set; }
        public int? AddressCountryId { get; set; }
        public CountryDto AddressCountry { get; set; }
        public int? AddressCityId { get; set; }
        public CityDto AddressCity { get; set; }
        public int? AddressRegionId { get; set; }
        public RegionDto AddressRegion { get; set; }
        public string AddressMuncipality { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool Employed { get; set; }
        public string EmployerName { get; set; }
        public int? EmployerCountryId { get; set; }
        public CountryDto EmployerCountry { get; set; }
        public int? EmployerCityId { get; set; }
        public CityDto EmployerCity { get; set; }
        public int? EmployerRegionId { get; set; }
        public RegionDto EmployerRegion { get; set; }
        public string EmployerAddress { get; set; }
        public string EmployerPhone { get; set; }
        public string EmployerMobile { get; set; }
        public string EmployerEmail { get; set; }
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
        public string EnrolmentDate { get; set; }
        public string FinishedSchool { get; set; }
        public string CertificateNumber { get; set; }
        public string SchoolLevel { get; set; }
        public string Vocation { get; set; }
        public bool Gdpr { get; set; }
        public string Notes { get; set; }

        public List<FileDto> Files { get; set; }
    }
}