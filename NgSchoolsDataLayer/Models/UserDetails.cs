﻿using NgSchoolsDataLayer.Models.BaseTypes;
using System;
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
        public string Mobile { get; set; }
        public string Mobile2 { get; set; }
        public string Phone { get; set; }
        public string Oib { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
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
    }
}
